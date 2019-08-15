using System;
using System.Collections.Generic;
using System.Text;

namespace TravelExpenses.Core
{
    public class Comentarios
    {
        public Comentarios()
        {
            this.estatus = "";
        }
        public int IdComentarios { get; set; }
        public string Comentario { get; set; }
        public int Folio { get; set; }
        public string estatus { get; set; }

        public static implicit operator Comentarios(List<Comentarios> v)
        {
            throw new NotImplementedException();
        }
    }
}
