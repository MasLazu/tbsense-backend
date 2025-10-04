using MasLazu.AspNet.Framework.EfCore;
using MasLazu.AspNet.Framework.EfCore.Postgresql.Data;
using TbSense.Backend.EfCore;
using TbSense.Backend.EfCore.Data;

namespace TbSense.Backend.Migrator;

public class TbSenseBackendDbContextFactory : PsqlDesignTimeDbContextFactory<TbSenseBackendDbContext>
{
    public override string GetMigrationsAssemblyName() => "TbSense.Backend.Migrator";
}