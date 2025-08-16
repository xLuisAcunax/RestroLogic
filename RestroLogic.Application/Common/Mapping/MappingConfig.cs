using Mapster;

namespace RestroLogic.Application.Common.Mapping
{
    public static class MappingConfig
    {
        public static void Register()
        {
            TypeAdapterConfig.GlobalSettings.Default.PreserveReference(true);
            // registrar mapeos específicos por módulo.
        }
    }
}
