import json
import sys
import uuid
from urllib.error import HTTPError
from urllib.request import Request, urlopen

base = (sys.argv[1] if len(sys.argv) > 1 else "http://localhost:8080").rstrip("/")
suffix = uuid.uuid4().hex[:12]


def request(method, path, payload=None, expected=200):
    data = json.dumps(payload).encode("utf-8") if payload is not None else None
    req = Request(
        f"{base}{path}",
        data=data,
        method=method,
        headers={"Content-Type": "application/json"} if data else {},
    )
    try:
        with urlopen(req, timeout=15) as response:
            status = response.status
            body = json.load(response)
    except HTTPError as error:
        status = error.code
        body = json.load(error)
    assert status == expected, f"{method} {path}: expected {expected}, got {status}: {body}"
    return body


assert request("GET", "/health")["status"] == "ok"
seller = request("POST", "/api/users", {
    "fullName": "Test Seller",
    "email": f"seller-{suffix}@example.com",
    "role": "Seller",
}, expected=201)
buyer = request("POST", "/api/users", {
    "fullName": "Test Buyer",
    "email": f"buyer-{suffix}@example.com",
    "role": "Buyer",
}, expected=201)
store = request("POST", "/api/stores", {
    "ownerId": seller["id"],
    "name": f"Store {suffix}",
}, expected=201)
category = request("POST", "/api/categories", {
    "name": f"Category {suffix}",
}, expected=201)
product = request("POST", "/api/products", {
    "sellerId": seller["id"],
    "storeId": store["id"],
    "categoryId": category["id"],
    "name": f"Laptop {suffix}",
    "price": 1999.99,
    "stockQuantity": 7,
}, expected=201)

assert any(item["id"] == store["id"] for item in request("GET", "/api/stores"))
assert any(item["id"] == product["id"] for item in request("GET", f"/api/stores/{store['id']}/products"))
found = request("GET", f"/api/products?search={suffix}&categoryId={category['id']}&minPrice=1000&maxPrice=2500")
assert found["totalCount"] == 1
assert found["items"][0]["id"] == product["id"]

order = request("POST", "/api/orders", {
    "buyerId": buyer["id"],
    "productId": product["id"],
    "quantity": 3,
}, expected=201)
assert order["totalPrice"] == 5999.97
assert request("GET", f"/api/products/{product['id']}")["stockQuantity"] == 4
assert any(item["id"] == order["id"] for item in request("GET", f"/api/users/{buyer['id']}/orders"))
request("POST", "/api/orders", {
    "buyerId": buyer["id"],
    "productId": product["id"],
    "quantity": 5,
}, expected=409)
assert request("GET", f"/api/products/{product['id']}")["stockQuantity"] == 4
print("PASS: marketplace smoke test")
