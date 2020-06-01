﻿using Archivos;
using Excepciones;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Clases_Instanciables
{
    public class Universidad
    {
        #region Enumerados
        public enum EClases
        {
            Programacion,
            Laboratorio,
            Legislacion,
            SPD
        }
        #endregion

        #region Atributos
        private List<Alumno> listaAlumnos;//lista de inscriptos
        private List<Jornada> listaJornadas;
        private List<Profesor> listaProfesores;//lista de quienes pueden dar clase.
        #endregion

        #region Properties
        public List<Alumno> Alumnos
        {
            get
            {
                return this.listaAlumnos;
            }
            set
            {
                this.listaAlumnos = value;
            }
        }
        public List<Jornada> Jornada
        {
            get
            {
                return this.listaJornadas;
            }
            set
            {
                this.listaJornadas = value;
            }
        }
        public Jornada this[int i]
        {
            get
            {
                return this.listaJornadas[i];
            }
            set
            {
                this.listaJornadas[i] = value;
            }
        }
        public List<Profesor> Instructores
        {
            get
            {
                return this.listaProfesores;
            }
            set
            {
                this.listaProfesores = value;
            }
        }
        #endregion

        #region Constructores
        public Universidad()
        {
            this.listaAlumnos = new List<Alumno>();
            this.listaJornadas = new List<Jornada>();
            this.listaProfesores = new List<Profesor>();
        }
        #endregion

        #region Métodos
        private static string MostrarDatos(Universidad uni)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("JORNADA:\n");
            foreach (Jornada item in uni.Jornada)
            {
                sb.AppendFormat(item.ToString());
                sb.AppendFormat("<-------------------------------------------------->\r\n\n");
            }
            return sb.ToString();
        }

        public static bool Guardar(Universidad uni)
        {
            Xml<Universidad> archivoXmlUni = new Xml<Universidad>();
            string ruta = Directory.GetCurrentDirectory() + @"\Universidad.xml";
            bool pudoGuardar = false;

            if (archivoXmlUni.Guardar(ruta, uni))
                pudoGuardar = true;

            return pudoGuardar;
        }

        public Universidad Leer()
        {
            Xml<Universidad> archivoXmlUni = new Xml<Universidad>();
            string ruta = Directory.GetCurrentDirectory() + @"\Universidad.xml";

            archivoXmlUni.Leer(ruta, out Universidad uni);

            return uni;
        }
        #endregion

        #region Operaciones
        public static bool operator ==(Universidad g, Alumno a)
        {
            bool inscripto = false;
            if (!(g is null))
            {
                foreach (Alumno item in g.Alumnos)
                {
                    if (item == a)
                    {
                        inscripto = true;
                        break;
                    }
                }
            }
            else
                throw new NullReferenceException("Universidad Nula -- EN: Universidad == Alumno");

            return inscripto;
        }

        public static bool operator !=(Universidad g, Alumno a)
        {
            return !(g == a);
        }

        public static bool operator ==(Universidad g, Profesor i)
        {
            bool daClases = false;
            if (!(g is null))
            {
                foreach (Profesor item in g.Instructores)
                {
                    if (item == i)
                    {
                        daClases = true;
                        break;
                    }
                }
            }
            else
                throw new NullReferenceException("Universidad Nula -- EN: Universidad == Profesor");

            return daClases;
        }

        public static bool operator !=(Universidad g, Profesor i)
        {
            return !(g == i);
        }

        public static Profesor operator ==(Universidad u, EClases clase)
        {
            Profesor primerProfeCapazDeDarEsaClase = null;
            if (!(u is null))
            {
                foreach (Profesor item in u.Instructores)
                {
                    if (item == clase)
                    {
                        primerProfeCapazDeDarEsaClase = item;
                        //el primer profe que SI pueda dar la clase lo retorno.
                        break;
                    }
                }
                if (primerProfeCapazDeDarEsaClase is null)
                    throw new SinProfesorException();
            }
            //else
            //    throw new NullReferenceException("Universidad Nula -- EN: Universidad == Clase");

            return primerProfeCapazDeDarEsaClase;
        }

        public static Profesor operator !=(Universidad u, EClases clase)
        {
            Profesor primerProfeQueNoPuedaDarLaClase = null;
            if (!(u is null))
            {
                foreach (Profesor item in u.Instructores)
                {
                    if (item != clase)
                    {
                        primerProfeQueNoPuedaDarLaClase = item;
                        //el primer profe que NO pueda dar la clase lo retorno.
                        break;
                    }
                }
            }
            //else
            //    throw new NullReferenceException("Universidad Nula -- EN: Universidad != Clase");

            return primerProfeQueNoPuedaDarLaClase;
        }


        public static Universidad operator +(Universidad u, Alumno a)
        {
            if (u != a)
                u.Alumnos.Add(a);
            else
                throw new AlumnoRepetidoException();

            return u;
        }


        public static Universidad operator +(Universidad u, Profesor i)
        {
            if (u != i)
                u.Instructores.Add(i);

            return u;
        }
        public static Universidad operator +(Universidad g, EClases clase)
        {
            if(!(g is null))
            {
                Profesor auxProfesor = (g == clase);
                //Creo jornada. (SI NO OCURRIÓ NINGUNA EXCEPTION)
                Jornada auxJornada = new Jornada(clase, auxProfesor);

                foreach (Alumno alumnito in g.Alumnos)
                {
                     if (alumnito == clase)//si el alumno cumpleRequisitos entra
                        auxJornada += alumnito;//agrego el alumno a la jornada.
                }

                if (!(auxJornada is null))
                    g.Jornada.Add(auxJornada);//agrego la jornada a la universidad.
            }

            return g;
        }
        #endregion

        #region Override
        public override string ToString()
        {
            return Universidad.MostrarDatos(this);
        }
        #endregion

    }
}
