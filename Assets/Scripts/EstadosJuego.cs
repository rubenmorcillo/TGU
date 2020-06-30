public static class EstadosJuego
{
    private static bool iniciado;
    private static bool menuActivo;

    public enum Estado {COMBATE , EXPLORAR, TIENDA , MENU};
    

    private static Estado estadoActual;

    

    public static void activarEstado(Estado e)
    {
        estadoActual = e;
    }

    public static Estado EstadoActual()
    {
        return estadoActual;
    }

    public static void setIniciado(bool b)
    {
        iniciado = b;
       
    }
    public static bool Iniciado()
    {
        return iniciado;
    }

    public static void setMenuActivo(bool b)
	{
        menuActivo = b;
	}
    public static bool MenuActivo()
	{
        return menuActivo;
	}
   
}
