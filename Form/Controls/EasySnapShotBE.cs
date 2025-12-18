using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyControlWeb.Form.Controls
{
    public class EasySnapShotBE
    {
        public EasySnapShotBE() : this(0, String.Empty, null) { }
        public EasySnapShotBE(int _IdRef, string _Descripcion, byte[] imgByteArray)
        {
            this.IdRef = _IdRef;
            this.Descripcion = _Descripcion;
            this.ImgByteArray = imgByteArray;
        }
        public EasySnapShotBE(string _Descripcion, byte[] imgByteArray)
        {
            this.Descripcion = _Descripcion;
            this.ImgByteArray = imgByteArray;
        }
        public int Id { get; set; }
        public int IdRef { get; set; }
        public string Descripcion { get; set; }
        public byte[] ImgByteArray { get; set; }

        public string LocalStorage { get; set; }

    }
}
