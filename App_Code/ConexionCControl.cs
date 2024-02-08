using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;
using System.Web;

/// <summary>
/// Descripción breve de ConexionCControl
/// </summary>
public class ConexionCControl
{
    /// <summary>
    /// Crea una instancia del acceso a la Base de Datos
    /// </summary>
	public ConexionCControl()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
        // Llamado al método conectar
        conectar();
	}
    MySqlConnection conn;
    MySqlCommand comando = new MySqlCommand();

    

    /// <summary>
    /// Establece la conexión con la Base de Datos definida en el archivo APP.CONFIG
    /// de la aplicación
    /// </summary>
    public void conectar()
    {
        try
        {
            string nombrecadena = "cloudcontrol";
            //if (HttpContext.Current.Request.ApplicationPath.Remove(0, 1) != "")
            //    nombrecadena = HttpContext.Current.Request.ApplicationPath.Remove(0, 1);

            if (conn == null)
            {
                conn = new MySqlConnection(ConfigurationManager.ConnectionStrings[nombrecadena].ToString());

            }
            else
            {
                if (conn.State != ConnectionState.Closed) conn.Close();
                conn = null;
                conn = new MySqlConnection(ConfigurationManager.ConnectionStrings[nombrecadena].ToString());
            }
        }
        catch (MySqlException e)
        {
            // Lanza la excepción en caso de no poder conectarse a la BD
            throw new ApplicationException("Error conectando a base de datos, consulte con su administrador de red", e);
        }
    }

    public bool probarconexion()
    {
        try
        {
            conectar();
            conn.Open();
            return true;
        }
        catch (MySqlException e)
        {
            return false;
        }
    }

    public void desconectar()
    {
        conn.Close();
    }


    /// <summary>
    /// Devuelve un DATATABLE con los registros que resultan
    /// de la ejecución de la consulta SQL pasada como parámetro
    /// </summary>
    /// <returns>DataTable con los registros resultado de la ejecución del comando</returns>
    public DataTable traerdata()
    {
        try
        {
            // Llama al método conectar para establecer a conexión
            conectar();
            // el método Open abre la conexión
            conn.Open();
            // instancia un datatable
            DataTable dtabtrae = new DataTable();
            // se crea el adaptador de datos
            MySqlDataAdapter adaptrae = new MySqlDataAdapter(comando.CommandText, conn);
            // el adaptador llena el datatable
            adaptrae.Fill(dtabtrae);
            // se cierra la conexión
            
            // se devuelve el datatable
            return dtabtrae;
        }
        catch (MySqlException e)
        {
            if (e.Number == 1042)
            {
                // Error de conexión
                throw new ApplicationException("Error conectando a base de datos, consulte con su administrador de red", e);
            }
            return null;
        }
        finally
        {
            conn.Close();
            conn.Dispose();
        }

    }

    public DataSet traerset()
    {
        try
        {
            // Llama al método conectar para establecer a conexión
            conectar();
            // el método Open abre la conexión
            conn.Open();
            // instancia un datatable
            DataSet ds = new DataSet();
            // se crea el adaptador de datos
            MySqlDataAdapter adaptrae = new MySqlDataAdapter(comando.CommandText, conn);
            // el adaptador llena el datatable
            adaptrae.Fill(ds);
            // se cierra la conexión
            conn.Close();
            // se devuelve el datatable
            return ds;
        }
        catch (MySqlException e)
        {
            if (e.Number == 1042)
            {
                // Error de conexión
                throw new ApplicationException("Error conectando a base de datos, consulte con su administrador de red", e);
            }
            return null;
        }

    }

    /// <summary>
    /// Devuelve un DATAROW con el registro que resulte
    /// de la ejecución de la consulta SQL pasada como parámetro
    /// </summary>
    /// <param name="cad">Consulta SQL a ejecutar</param>
    /// <returns></returns>
    public DataRow traerfila()
    {
        try
        {
            conectar();
            conn.Open();
            DataTable dtabtrae = new DataTable();
            MySqlDataAdapter adaptrae = new MySqlDataAdapter(comando.CommandText, conn);
            adaptrae.Fill(dtabtrae);
                      

            // Se verifica la cantidad de registros del datatable
            if (dtabtrae.Rows.Count > 0)
            {
                // Se asigna al datarow el primer registro del datatable
                // y se devuelve
                DataRow dfiltrae = dtabtrae.Rows[0];
                return dfiltrae;
            }
            else
            {
                // En caso de que la consulta no devuelva resultados se
                // retorna null
                return null;
            }
        }
        catch (MySqlException e)
        {
            throw (e);
            //if (e.Number == 1042)
            //{
            //throw new ApplicationException("Error conectando a base de datos, consulte con su administrador de red", e);
            //}
            return null;
        }
        finally
        {
            conn.Close();
            conn.Dispose();
        }
    }

    /// <summary>
    /// Ejecuta una consulta de acción (INSERT, UPDATE, DELETE)
    /// sobre la BD
    /// </summary>
    /// <param name="cad">Consulta SQL a ejecutar</param>
    /// <returns></returns>
    public Boolean guardadata()
    {
        try
        {
            conectar();
            conn.Open();
            MySqlCommand cmdguarda = new MySqlCommand(comando.CommandText, conn);
            cmdguarda.ExecuteNonQuery();
            
            cmdguarda.Connection.Close();
            return true;
        }
        catch (MySqlException e)
        {
            if (e.Number == 1042)
            {
                throw new ApplicationException("Error conectando a base de datos, consulte con su administrador de red", e);
            }
            return false;
        }
        finally
        {
            conn.Close();
            conn.Dispose();
        }
    }

    //Ejecuta una consulta de acción (INSERT, UPDATE, DELETE)
    //sobre la BD, y en caso de AUTO INC me trae el codigo
    public long guardadataid()
    {
        try
        {
            long idgen;
            conectar();
            conn.Open();
            MySqlCommand cmdguarda = new MySqlCommand(comando.CommandText, conn);
            cmdguarda.ExecuteNonQuery();
            idgen = cmdguarda.LastInsertedId;
            conn.Close();
            cmdguarda.Connection.Close();
            return idgen;
        }
        catch (MySqlException e)
        {
            if (e.Number == 1042)
            {
                throw new ApplicationException("Error conectando a base de datos, consulte con su administrador de red", e);
            }
            return -1;
        }
        finally
        {
            conn.Close();
            conn.Dispose();
        }
    }


    /// <summary>
    /// Crea un comando en base a una sentencia SQL.
    /// Ejemplo:
    /// <code>SELECT * FROM Tabla WHERE campo1=@campo1, campo2=@campo2</code>
    /// Guarda el comando para el seteo de parámetros y la posterior ejecución.
    /// </summary>
    /// <param name="sentenciaSQL">La sentencia SQL con el formato: SENTENCIA [param = @param,]</param>

    public void CrearComando(string sentenciaSQL)
    {
        this.comando.CommandType = CommandType.Text;
        this.comando.CommandText = sentenciaSQL;
    }

    /// <summary>
    /// Setea un parámetro como nulo del comando creado.
    /// </summary>
    /// <param name="nombre">El nombre del parámetro cuyo valor será nulo.</param>
    public void AsignarParametroNulo(string nombre)
    {
        AsignarParametro(nombre, "", "NULL");
    }

    /// <summary>
    /// Asigna un parámetro de tipo cadena al comando creado.
    /// </summary>
    /// <param name="nombre">El nombre del parámetro.</param>
    /// <param name="valor">El valor del parámetro.</param>
    public void AsignarParametroCadena(string nombre, string valor)
    {
        AsignarParametro(nombre, "'", valor);
    }

    /// <summary>
    /// Asigna un parámetro de tipo entero al comando creado.
    /// </summary>
    /// <param name="nombre">El nombre del parámetro.</param>
    /// <param name="valor">El valor del parámetro.</param>
    public void AsignarParametroEntero(string nombre, int valor)
    {
        AsignarParametro(nombre, "", valor.ToString());
    }


    /// <summary>
    /// Asigna un parámetro de tipo doble al comando creado.
    /// </summary>
    /// <param name="nombre">El nombre del parámetro.</param>
    /// <param name="valor">El valor del parámetro.</param>
    public void AsignarParametroDouble(string nombre, double valor)
    {
        AsignarParametro(nombre, "", valor.ToString());
    }

    /// <summary>
    /// Asigna un parámetro al comando creado.
    /// </summary>
    /// <param name="nombre">El nombre del parámetro.</param>
    /// <param name="separador">El separador que será agregado al valor del parámetro.</param>
    /// <param name="valor">El valor del parámetro.</param>
    private void AsignarParametro(string nombre, string separador, string valor)
    {
        int indice = this.comando.CommandText.IndexOf(nombre);
        string prefijo = this.comando.CommandText.Substring(0, indice);
        string sufijo = this.comando.CommandText.Substring(indice + nombre.Length);
        this.comando.CommandText = prefijo + separador + valor + separador + sufijo;
    }

    /// <summary>
    /// Asigna un parámetro de tipo fecha al comando creado.
    /// </summary>
    /// <param name="nombre">El nombre del parámetro.</param>
    /// <param name="valor">El valor del parámetro.</param>
    public void AsignarParametroFecha(string nombre, DateTime valor)
    {
        AsignarParametro(nombre, "'", valor.ToString());
    }
}