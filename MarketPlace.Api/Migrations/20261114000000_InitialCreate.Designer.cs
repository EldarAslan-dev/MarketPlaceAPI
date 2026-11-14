using MarketPlace.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketPlace.Api.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("20261114000000_InitialCreate")]
public partial class InitialCreate
{
    protected override void BuildTargetModel(ModelBuilder modelBuilder) => InitialModelBuilder.Build(modelBuilder);
}
