using System.ComponentModel;

namespace BusinessManager.Models.Enums
{
    public enum MovementTypeEnum
    {
        [Description("Entrada de inventario")]
        ENTRADA = 1,

        [Description("Salida de inventario")]
        SALIDA = 2,

        [Description("Ajuste de inventario")]
        AJUSTE = 3,

        [Description("Traslado entre bodegas")]
        TRASLADO = 4,

        [Description("Devolución")]
        DEVOLUCION = 5
    }
}
