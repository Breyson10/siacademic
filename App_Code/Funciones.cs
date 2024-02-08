using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Reflection;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Web.UI.WebControls;
using System.Runtime.InteropServices;
//using RestSharp;

/// <summary>
/// Descripción breve de Funciones
/// </summary>
public class Funciones
{
    //public static string ruta = "http://181.49.141.214:8080/apicloud_master/public";
    public static string ruta = "http://localhost:8080/apicloud_master/public";
    //public static string ruta = "http://localhost:90";
    public static string bd = "liceodecervantes";
    public static string rutaArchivo2 = "http://liceodecervantes.edu.co/oldweb/wp-content/uploads/";

    public Funciones()
    {

    }

    public static string nombreCadena()
    {
        string nombrecadena = "nas";//"localhost";//"testversion";//"localhost";//"tecvistahermosa"; //si está en local
        if (HttpContext.Current.Request.ApplicationPath.Remove(0, 1) != "")
            nombrecadena = HttpContext.Current.Request.ApplicationPath.Remove(0, 1);
        return nombrecadena;
    }
    public static string nombreCadena2()
    {
        string nombrecadena = nombreCadena() == "localhost" ? "conMySql" : nombreCadena();//"localhost";//"testversion";//"localhost";//"tecvistahermosa"; //si está en local
        if (HttpContext.Current.Request.ApplicationPath.Remove(0, 1) != "")
            nombrecadena = HttpContext.Current.Request.ApplicationPath.Remove(0, 1);
        return nombrecadena;
    }
    public static string nombreCadenaRedireccion()
    {
        //Llenamos la cadena de conexión con el proyecto actual
        string cadenaConexion = nombreCadena() == "localhost" ? "conMySql" : nombreCadena();
        //cadenaConexion = "conMysql";

        //Validamos si el proyecto actual tiene redirección
        // Configuraciones con = new Configuraciones();
        DataRow datoRedireccion = null;
        if (datoRedireccion != null && datoRedireccion["cadenaredireccion"] != null
            && datoRedireccion["cadenaredireccion"].ToString() != "")
        {
            cadenaConexion = datoRedireccion["cadenaredireccion"].ToString();
        }

        return cadenaConexion;
    }

    public string colocarPalabraenMayuscula(string palabra)
    {
        palabra = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(palabra.ToLower());
        return palabra;
    }

    //Funciones de Fecha y Hora
    public string getHoraSinSegundosActual()
    {
        DateTime localDateTime = DateTime.Now;
        DateTime utcDateTime = localDateTime.ToUniversalTime().AddHours(-5);
        string horares = utcDateTime.ToString("HH:mm");
        return horares;
    }

    public static string getString(Object objeto, string defecto = "")
    {
        if (objeto != null && objeto.ToString() != "")
            return objeto.ToString();
        return defecto;
    }

    public static string getTextDrop(DropDownList drop, string defecto = "")
    {
        if (drop.SelectedIndex > 0)
        {
            string cadena = drop.SelectedItem.Text;
            if (!String.IsNullOrWhiteSpace(cadena) && cadena != "Seleccione")
            {
                return cadena;
            }
        }
        return defecto;
    }

    /// <summary>
    /// Devuelve True si hay datos
    /// </summary>
    public static Boolean Valid(DataTable tabla)
    {
        if (tabla != null && tabla.Rows.Count > 0)
            return true;
        return false;
    }

    public static Boolean ValidRow(DataRow row)
    {

        if (row != null && row.ItemArray[0].ToString() != "")
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Devuelve true si la data es diferente de null o vacia
    /// </summary>
    public static bool noVacio(object data)
    {
        if (data != null && data.ToString() != "")
            return true;
        return false;
    }
    public string getHoraActual()
    {
        DateTime localDateTime = DateTime.Now;
        DateTime utcDateTime = localDateTime.ToUniversalTime().AddHours(-5);
        string horares = utcDateTime.ToString("HH:mm:ss");
        return horares;
    }
    public string getFechaAñoHoraActual()
    {
        DateTime localDateTime = DateTime.Now;
        DateTime utcDateTime = localDateTime.ToUniversalTime().AddHours(-5);
        string horares = utcDateTime.ToString("yyyy-MM-dd HH:mm:ss");
        return horares;
    }
    public string getFechaAñoHoraActual(string zonaHoraria)
    {
        DateTime utcDateTime = DateTime.Now.ToUniversalTime().AddHours(Convert.ToDouble(zonaHoraria));       
        return utcDateTime.ToString("yyyy-MM-dd HH:mm:ss");        
    }
    public string getFechaAñoActual()
    {
        string fecha = String.Format("{0:yyyy-MM-dd}", DateTime.Now.Date);
        return fecha;
    }
    public string getFechaDiaActual()
    {
        string fecha = String.Format("{0:dd-MM-yyyy}", DateTime.Now.Date);
        return fecha;
    }
    public string convertFechaAño(string fechadia)
    {
        string fecha = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(fechadia));
        return fecha;
    }
    public string convertFechaAñoVacio(string fechadia)
    {
        if (!String.IsNullOrEmpty(fechadia))
        {
            string fecha = String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(fechadia));
            return fecha;
        }
        return "";
    }
    public string convertFechaDia(string fechaaño)
    {
        if (fechaaño != "")
        {
            string fecha = String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(fechaaño));
            return fecha;
        }
        else
        {
            return fechaaño = "";
        }
    }
    public string convertFechaDiaGuionInclinado(string fechaaño)
    {
        if (fechaaño != "")
        {
            string fecha = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(fechaaño));
            return fecha;
        }
        else
        {
            return fechaaño = "";
        }
    }
    public string convertFechaHoraDia(string fechahoraaño)
    {
        if (fechahoraaño != "")
        {
            string fecha = String.Format("{0:dd-MM-yyyy HH:mm:ss}", Convert.ToDateTime(fechahoraaño));
            return fecha;
        }
        else
        {
            return fechahoraaño = "";
        }
    }
    public string convertFechaHoraDiaMinutos(string fechahoraaño)
    {
        if (fechahoraaño != "")
        {
            string fecha = String.Format("{0:dd-MM-yyyy HH:mm}", Convert.ToDateTime(fechahoraaño));
            return fecha;
        }
        else
        {
            return fechahoraaño = "";
        }
    }

    public string convertFechaAñoMesDiaHora(string fechahoraaño)
    {
        if (fechahoraaño != "")
        {
            string fecha = String.Format("{0:yyyy-MM-dd HH:mm:ss}", Convert.ToDateTime(fechahoraaño));
            return fecha;
        }
        else
        {
            return fechahoraaño = "";
        }
    }

    public static string Formato(DateTime? datetime, string format = null)
    {
        string fechaFormat = "";
        string formatDefault = "dd-MM-yyyy";
        // Validamos si viene fecha o puede venir null
        if (datetime.HasValue)
        {
            if (!String.IsNullOrWhiteSpace(format))
                formatDefault = format;

            // Intentamos hacer el formato
            try { fechaFormat = Convert.ToDateTime(datetime.ToString()).ToString(formatDefault); }
            catch (Exception) { return ""; }

            // Si todo salio bien Retornamos
            return fechaFormat;
        }
        return "";
    }

    public string convertHoraSinSegundos(string fechahora)
    {
        string fecha = String.Format("{0:HH:mm}", Convert.ToDateTime(fechahora));
        return fecha;
    }

    public string enletras(string num)
    {
        string res, dec = "";
        Int64 entero;
        int decimales;
        double nro;
        try
        {
            nro = Convert.ToDouble(num);
        }
        catch
        {
            return "";
        }
        entero = Convert.ToInt64(Math.Truncate(nro));
        decimales = Convert.ToInt32(Math.Round((nro - entero) * 100, 2));
        if (decimales > 0)
        {
            dec = " CON " + decimales.ToString() + "/100";
        }
        res = toText(Convert.ToDouble(entero)) + dec;
        return res;
    }
    private string toText(double value)
    {
        string Num2Text = "";
        value = Math.Truncate(value);
        if (value == 0) Num2Text = "CERO";
        else if (value == 1) Num2Text = "uno";
        else if (value == 2) Num2Text = "dos";
        else if (value == 3) Num2Text = "tres";
        else if (value == 4) Num2Text = "cuatro";
        else if (value == 5) Num2Text = "cinco";
        else if (value == 6) Num2Text = "seis";
        else if (value == 7) Num2Text = "siete";
        else if (value == 8) Num2Text = "ocho";
        else if (value == 9) Num2Text = "nueve";
        else if (value == 10) Num2Text = "diez";
        else if (value == 11) Num2Text = "once";
        else if (value == 12) Num2Text = "doce";
        else if (value == 13) Num2Text = "trece";
        else if (value == 14) Num2Text = "catorce";
        else if (value == 15) Num2Text = "quince";
        else if (value < 20) Num2Text = "dieci" + toText(value - 10);
        else if (value == 20) Num2Text = "veinte";
        else if (value < 30) Num2Text = "veinti" + toText(value - 20);
        else if (value == 30) Num2Text = "treinta";
        else if (value == 40) Num2Text = "cuarenta";
        else if (value == 50) Num2Text = "cincuenta";
        else if (value == 60) Num2Text = "sesenta";
        else if (value == 70) Num2Text = "setenta";
        else if (value == 80) Num2Text = "ochenta";
        else if (value == 90) Num2Text = "noventa";
        else if (value < 100) Num2Text = toText(Math.Truncate(value / 10) * 10) + " y " + toText(value % 10);
        else if (value == 100) Num2Text = "cien";
        else if (value < 200) Num2Text = "ciento " + toText(value - 100);
        else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) Num2Text = toText(Math.Truncate(value / 100)) + "cientos";
        else if (value == 500) Num2Text = "quinientos";
        else if (value == 700) Num2Text = "setecientos";
        else if (value == 900) Num2Text = "novecientos";
        else if (value < 1000) Num2Text = toText(Math.Truncate(value / 100) * 100) + " " + toText(value % 100);
        else if (value == 1000) Num2Text = "mil";
        else if (value < 2000) Num2Text = "mil " + toText(value % 1000);
        else if (value < 1000000)
        {
            Num2Text = toText(Math.Truncate(value / 1000)) + " mil";
            if ((value % 1000) > 0) Num2Text = Num2Text + " " + toText(value % 1000);
        }
        else if (value == 1000000) Num2Text = "un millon";
        else if (value < 2000000) Num2Text = "un millon " + toText(value % 1000000);
        else if (value < 1000000000000)
        {
            Num2Text = toText(Math.Truncate(value / 1000000)) + " millones ";
            if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000) * 1000000);
        }
        else if (value == 1000000000000) Num2Text = "un billon";
        else if (value < 2000000000000) Num2Text = "un billon " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);
        else
        {
            Num2Text = toText(Math.Truncate(value / 1000000000000)) + " billones";
            if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);
        }
        return Num2Text;
    }

    public Boolean email_bien_escrito(String email)
    {
        String expresion;
        expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
        if (Regex.IsMatch(email, expresion))
        {
            if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                return true;
            else
                return false;
        }
        else
            return false;

    }
    public string obtenerNombreMesNumero(int numeroMes)
    {
        try
        {
            DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
            string nombreMes = formatoFecha.GetMonthName(numeroMes);
            return nombreMes;
        }
        catch
        {
            return "Desconocido";
        }
    }
    public string convertirNumeroRomanos(string numero)
    {
        string ca = "";
        switch (numero)
        {
            case "1": ca = "I"; break;
            case "2": ca = "II"; break;
            case "3": ca = "III"; break;
            case "4": ca = "IV"; break;
            case "5": ca = "V"; break;
            case "6": ca = "VI"; break;
            case "7": ca = "VII"; break;
            case "8": ca = "VIII"; break;
            case "9": ca = "IX"; break;
            case "10": ca = "X"; break;
        }
        return ca;
    }
    public string convertirNombreCursoNumeroCurso(string grado)
    {
        string ca = "";
        switch (grado)
        {
            case "PRIMERO": ca = "1"; break;
            case "SEGUNDO": ca = "2"; break;
            case "TERCERO": ca = "3"; break;
            case "CUARTO": ca = "4"; break;
            case "QUINTO": ca = "5"; break;
            case "SEXTO": ca = "6"; break;
            case "SEPTIMO": ca = "7"; break;
            case "OCTAVO": ca = "8"; break;
            case "NOVENO": ca = "9"; break;
            case "DECIMO": ca = "10"; break;
            case "UNDECIMO": ca = "11"; break;
        }
        return ca;
    }

    public static string reemplazarPeligrosasEtiquetasHTML(string html) {
        html = html.Trim();
        html = html.Replace("<body", "<p");
        html = html.Replace("</body", "</p");
        html = html.Replace("<style", "<p");
        html = html.Replace("</style", "</p");
        html = html.Replace("<script", "<p");
        html = html.Replace("</script", "</p");
        return html;
    }

    /// <summary>
    /// Obtiene toda la información de un archivo
    /// La variable Imagen de tipo bool, limita las extensiones de los archivos. (.jpg,.png)
    /// </summary>
    /// <param name="trepador"></param>
    /// <param name="Imagen"></param>
    /// <returns></returns>

    public string validarLongitudString(string cadena, int longitudMax)
    {
        string ca = "";

        if (cadena != null)
        {
            if (cadena.Length > longitudMax)
                ca = cadena.Substring(0, longitudMax);
            else
                ca = cadena;
        }

        return ca;
    }

    public static string GetMD5(string str)
    {
        MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
        byte[] bs = System.Text.Encoding.UTF8.GetBytes(str);
        bs = x.ComputeHash(bs);
        System.Text.StringBuilder s = new System.Text.StringBuilder();
        foreach (byte b in bs)
        {
            s.Append(b.ToString("x2").ToLower());
        }
        String hash = s.ToString();
        return hash;
    }

    public static string buscarMunicipio()
    {
        string nombreCadena = Funciones.nombreCadena();
        //nombreCadena = "sanjose";
        switch (nombreCadena)
        {
            case "cis":
                return "Cartagena";

            case "ellibano":
            case "ellibanonoc":
                return "Santa Marta";

            case "gimnasiodangond":
                return "Valledupar";

            case "antonioramon":
            case "tecvistahermosa":
            case "noroccidentalsoledad":
            case "tajamar":
            case "sagradasabiduria":
            case "cefi":
                return "Soledad";

            case "teccomercialsabanalarga":
            case "santateresita":
            case "santateresitaciclo":
                return "Sabanalarga";

            case "sanjoseaguada":
                return "Aguada de Pablo";

            case "lcr":
            case "gcb":
            case "jgcb":
            case "colnacidosencristo":
                return "Bogotá D.C.";

            case "nuevaflorida":
            case "nuevafloridanoc":
                return "San Andrés de Tumaco";

            case "gischool":
                return "Armenia";

            case "mauxlaceja":
                return "La Ceja"; 
            
            case "sanjose":
                return "Cajicá";

            case "inedmi":
                return "Sabanalarga";
            case "fem":
                return "Montelibano";
            default:
                return "Barranquilla";
            case "gimnasiocristianodecundinamarca":
                return "Funza";
            case "gimnasioaltair":
                return "Sincelejo ";
        }
    }

    public static string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }

    public static string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }

    public string quitarTildes(string inputString)
    {
        Regex replace_a_Accents = new Regex("[á|à|ä|â]", RegexOptions.Compiled);
        Regex replace_e_Accents = new Regex("[é|è|ë|ê]", RegexOptions.Compiled);
        Regex replace_i_Accents = new Regex("[í|ì|ï|î]", RegexOptions.Compiled);
        Regex replace_o_Accents = new Regex("[ó|ò|ö|ô]", RegexOptions.Compiled);
        Regex replace_u_Accents = new Regex("[ú|ù|ü|û]", RegexOptions.Compiled);
        inputString = replace_a_Accents.Replace(inputString, "a");
        inputString = replace_e_Accents.Replace(inputString, "e");
        inputString = replace_i_Accents.Replace(inputString, "i");
        inputString = replace_o_Accents.Replace(inputString, "o");
        inputString = replace_u_Accents.Replace(inputString, "u");
        return inputString;
    }


    public static string ComputeSha256Hash(string rawData)
    {
        // Create a SHA256   
        using (SHA256 sha256Hash = SHA256.Create())
        {
            // ComputeHash - returns byte array  
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            // Convert byte array to a string   
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
    public static string GetSHA1_Base64(string str)
    {
        SHA1 sha1 = SHA1Managed.Create();
        ASCIIEncoding encoding = new ASCIIEncoding();        
        byte[] stream = sha1.ComputeHash(encoding.GetBytes(str));      
        return Convert.ToBase64String(stream);
    }

    /// <summary>
    /// Si es insert no hiría el código.
    /// string[] encabezado = { "cod", "campo1", "campo3" };
    /// string[] value = { "1", "valor1", "valor3" };
    /// string[] respuesta2 = crudTabla("nombre_tabla", encabezado, value, "2");
    /// </summary>
    /// <param name="nombretabla"></param>
    /// <param name="encabezado"></param>
    /// <param name="value"></param>
    /// <param name="tipo"></param>
    /// <returns></returns>
    // public static string[] crudTabla(string nombretabla, string[] encabezado, string[] value, string tipo)
    // {
    //     string[] msj = new string[3];
    //     if (encabezado.Length > 0 && value.Length > 0)
    //     {
    //         ConexionMultiple conector = new ConexionMultiple(Funciones.nombreCadena2());
    //         switch (tipo)
    //         {
    //             case "1": //INSERT
    //                 string insert = "INSERT INTO " + nombretabla + " (";
    //                 for (int i = 0; i < encabezado.Length; i++)
    //                 {
    //                     insert += encabezado[i];
    //                     if (i + 1 < encabezado.Length)
    //                     {
    //                         insert += ",";
    //                     }
    //                 }
    //                 insert += ") VALUES (";
    //                 for (int i = 0; i < encabezado.Length; i++)
    //                 {
    //                     insert += "@" + encabezado[i];
    //                     if (i + 1 < encabezado.Length)
    //                     {
    //                         insert += ",";
    //                     }
    //                 }
    //                 insert += ")";
    //                 conector.CrearComando(insert);
    //                 for (int i = 0; i < encabezado.Length; i++)
    //                 {
    //                     conector.AsignarParametroCadenaoNull2("@" + encabezado[i], value[0]);
    //                     value = value.Where((source, index) => index != 0).ToArray();
    //                 }
    //                 long InsertCod = conector.guardadataid2();
    //                 if (InsertCod != -1)
    //                 {
    //                     msj[0] = "ok";
    //                     msj[1] = "Datos guardados correctamente";
    //                     msj[2] = InsertCod.ToString();
    //                 }
    //                 else
    //                 {
    //                     msj[0] = "no";
    //                     msj[1] = "No se lograron guardar los datos";                        
    //                 }                    
    //                 break;
    //             case "2": //UPDATE   
    //                 string codigo = value[0];
    //                 string consultaU = @"SELECT * FROM " + nombretabla + " WHERE " + encabezado[0] + "=@" + encabezado[0];
    //                 conector.CrearComando(consultaU);
    //                 conector.AsignarParametroCadena2("@" + encabezado[0], codigo);
    //                 DataRow dtRowU = conector.traerfila2();
    //                 if (dtRowU != null)
    //                 {
    //                     conector = new ConexionMultiple(Funciones.nombreCadena2());
    //                     string update = "UPDATE " + nombretabla + " SET ";
    //                     for (int i = 1; i < encabezado.Length; i++)
    //                     {
    //                         update += encabezado[i] + "=@" + encabezado[i];
    //                         if (i + 1 < encabezado.Length)
    //                         {
    //                             update += ",";
    //                         }
    //                     }
    //                     update += " WHERE " + encabezado[0] + "=@" + encabezado[0];
    //                     conector.CrearComando(update);
    //                     for (int i = 1; i < encabezado.Length; i++)
    //                     {
    //                         value = value.Where((source, index) => index != 0).ToArray();
    //                         conector.AsignarParametroCadenaoNull2("@" + encabezado[i], value[0]);
    //                     }
    //                     conector.AsignarParametroCadena2("@" + encabezado[0], codigo);
    //                     if (conector.guardadata2())
    //                     {
    //                         msj[0] = "ok";
    //                         msj[1] = "Datos actualizados correctamente";
    //                     }
    //                     else
    //                     {
    //                         msj[0] = "no";
    //                         msj[1] = "No se lograron actualizar los datos";
    //                     }
    //                 }
    //                 else
    //                 {
    //                     msj[0] = "no";
    //                     msj[1] = "No hay datos en la tabla para actualizar";
    //                 }
    //                 break;
    //             case "3": //DELETE
    //                 string consultaD = @"SELECT * FROM " + nombretabla + " WHERE " + encabezado[0] + "=@" + encabezado[0];
    //                 conector.CrearComando(consultaD);
    //                 conector.AsignarParametroCadena2("@" + encabezado[0], value[0]);
    //                 DataRow dtRowD = conector.traerfila2();
    //                 if (dtRowD != null)
    //                 {
    //                     conector = new ConexionMultiple(Funciones.nombreCadena2());
    //                     string delete = "DELETE FROM " + nombretabla + " WHERE " + encabezado[0] + "=@" + encabezado[0];
    //                     conector.CrearComando(delete);
    //                     conector.AsignarParametroCadena2("@" + encabezado[0], value[0]);
    //                     if (conector.guardadata2())
    //                     {
    //                         msj[0] = "ok";
    //                         msj[1] = "Datos eliminados correctamente";
    //                     }
    //                     else
    //                     {
    //                         msj[0] = "no";
    //                         msj[1] = "No se lograron eliminar los datos";
    //                     }
    //                 }
    //                 else
    //                 {
    //                     msj[0] = "no";
    //                     msj[1] = "No hay datos en la tabla para eliminar";
    //                 }
    //                 break;
    //         }
    //     }
    //     else
    //     {
    //         msj[0] = "no";
    //         msj[1] = "No hay parámetros";
    //     }
    //     return msj;
    // }

    public string convertirLetraDiaEnCodigo(string dia, string tipo)
    {
        string resp = "";
        if (dia != "")
        {
            if (tipo == "1")
            {
                switch (dia)
                {
                    case "Lunes":
                        resp = "1";
                        break;
                    case "Martes":
                        resp = "2";
                        break;
                    case "Miercoles":
                        resp = "3";
                        break;
                    case "Jueves":
                        resp = "4";
                        break;
                    case "Viernes":
                        resp = "5";
                        break;
                    case "Sabado":
                        resp = "6";
                        break;
                    case "Domingo":
                        resp = "7";
                        break;
                }
            }
            else if (tipo == "2")
            {
                switch (dia)
                {
                    case "1":
                        resp = "Lunes";
                        break;
                    case "2":
                        resp = "Martes";
                        break;
                    case "3":
                        resp = "Miercoles";
                        break;
                    case "4":
                        resp = "Jueves";
                        break;
                    case "5":
                        resp = "Viernes";
                        break;
                    case "6":
                        resp = "Sabado";
                        break;
                    case "7":
                        resp = "Domingo";
                        break;
                }
            }
            else
            {
                resp = "";
            }

        }
        return resp;
    }

    public bool compararFechas(string fechaini, string fechafin)
    {
        bool bien = false;

        DateTime dateini = Convert.ToDateTime(fechaini);
        DateTime datefin = Convert.ToDateTime(fechafin);
        int result = DateTime.Compare(dateini, datefin);

        if (result <= 0)
            bien = true;

        return bien;
    }

    /// <summary>
    /// AGREGADO POR NEIL VERGARA POR SI ACASO
    /// </summary>
    /// <param name="Source"></param>
    /// <param name="Start"></param>
    /// <param name="End"></param>
    /// <returns></returns>
    public static string stringBetween(string Source, string Start, string End)
    {
        string result = "";
        if (Source.Contains(Start) && Source.Contains(End))
        {
            int StartIndex = Source.IndexOf(Start, 0) + Start.Length;
            int EndIndex = Source.IndexOf(End, StartIndex);
            result = Source.Substring(StartIndex, EndIndex - StartIndex);
            return result;
        }

        return result;
    }
    public static string rutaCompletaDominioActual()
    {
        return System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Host + ":" + System.Web.HttpContext.Current.Request.Url.Port + System.Web.HttpContext.Current.Request.ApplicationPath; //Result: https://controlacademic.co:443/livingstone
    }

    public static string rutaCompletaDominioActual_Api(string nombreProyectoApi, string puertoLocal)
    {
        nombreProyectoApi = (nombreProyectoApi != "" ? "/" + nombreProyectoApi : "");

        return System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Host + ":" + (Funciones.nombreCadena() == "localhost" ? puertoLocal : System.Web.HttpContext.Current.Request.Url.Port.ToString()) + nombreProyectoApi + "/" + Funciones.nombreCadena();
    }

    public static bool EsBase64(string texto)
    {
        try
        {
            // Verificar si el texto contiene solo caracteres válidos de base64
            if (texto.Length % 4 != 0 || !System.Text.RegularExpressions.Regex.IsMatch(texto, @"^[a-zA-Z0-9\+/]*={0,2}$"))
                return false;

            // Intentar decodificar el texto en base64
            byte[] base64Bytes = Convert.FromBase64String(texto);

            // Si no se lanza una excepción, entonces el texto es válido en base64
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool ContieneCaracteresEspeciales(string texto)
    {
        // Define una expresión regular que busca cualquier carácter que no sea letra o número
        string expresionRegular = @"[^a-zA-Z0-9^,^.\s]";

        // Usa Regex.IsMatch para verificar si el texto contiene caracteres especiales
        return Regex.IsMatch(texto, expresionRegular);
    }

    // public static string buscarNombrePeriodo(string codperiodo)
    // {
    //     Periodos per = new Periodos();
    //     string nombrePeriodo = per.buscarPeriodoxCod(codperiodo)["numero"].ToString();
    //     switch (nombrePeriodo)
    //     {
    //         case "1":
    //             nombrePeriodo = "Primer";
    //             break;
    //         case "2":
    //             nombrePeriodo = "Segundo";
    //             break;
    //         case "3":
    //             nombrePeriodo = "Tercer";
    //             break;
    //         case "4":
    //             nombrePeriodo = "Cuarto";
    //             break;
    //         default:
    //             nombrePeriodo = "[No Encontrado]";
    //             break;
    //     }
    //     return nombrePeriodo;
    // }

    public static int calcularEdadEnAnio(string fechanacimiento)
    {
        int edad = 0;
        if (!String.IsNullOrEmpty(fechanacimiento))
        {
            DateTime fechaNacimiento = Convert.ToDateTime(fechanacimiento);

            // Obtener la fecha actual
            DateTime fechaActual = DateTime.Now;

            // Calcular la diferencia de años
            edad = fechaActual.Year - fechaNacimiento.Year;

            // Ajustar la edad si aún no ha tenido su cumpleaños este año
            if (fechaNacimiento > fechaActual.AddYears(-edad))
            {
                edad--;
            }
        }
        return edad;
    }

}