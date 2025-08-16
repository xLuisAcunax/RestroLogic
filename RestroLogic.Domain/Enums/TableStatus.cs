namespace RestroLogic.Domain.Enums
{

    public enum TableStatus
    {
        Available = 0,  // Libre para asignar
        Occupied = 1,  // Con orden abierta
        Blocked = 2,  // Fuera de servicio / reservada
        Cleaning = 3   // En limpieza (opcional)
    }
}
