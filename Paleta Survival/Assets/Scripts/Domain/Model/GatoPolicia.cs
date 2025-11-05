using BreakingCat_Project.Assets.Scripts.Domain.Model;

namespace BreakingCat_Project.Assets.Scripts.Domain.Model
{
    public class GatoPolicia : Gato
    {
        public int RadioBusqueda { get; set; }
        public int RadioDeteccion { get; set; }
        public Posicion[] Esquinas { get; set; }

        public GatoPolicia(string nombre, int radioBusqueda, int radioDeteccion, Posicion[] esquinas) : base()
        {
            this.Nombre = nombre;
            this.RadioBusqueda = radioBusqueda;
            this.RadioDeteccion = radioDeteccion;
            this.Esquinas = esquinas;
        }
    }
}