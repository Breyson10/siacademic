using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

public class ConexionMultiple
{
    MySqlConnection conn;
    MySqlTransaction transaccion;
    MySqlCommand comando = new MySqlCommand();
    private string nombrecadena {get; set; }

    /// <summary>
    /// Crea una instancia del acceso a la Base de Datos
    /// </summary>
    public ConexionMultiple(string nombrecadena)
    {
        // Llamado al método conectar
        this.nombrecadena = nombrecadena;
        conectar();
    }

    /// <summary>
    /// Establece la conexión con la Base de Datos definida en el archivo APP.CONFIG
    /// de la aplicación
    /// </summary>
    public void conectar()
    {
        try
        {
           //nombrecadena = "conMySql";
           // if (HttpContext.Current.Request.ApplicationPath.Remove(0, 1) != "")
           //     nombrecadena = HttpContext.Current.Request.ApplicationPath.Remove(0, 1);

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

    public void IniciarTransaccion()
    {
        // Abrimos la conexion
        this.conn.Open();

        // Inicializamos la transaccion
        transaccion = this.conn.BeginTransaction();

        // asignamos la transcion al comando
        this.comando.Transaction = transaccion;
    }
    public void TransaccionCommit()
    {
        // Todo salió bien guardamos los cambios
        transaccion.Commit();
        this.comando.Connection.Close();
        this.conn.Close();
    }
    public void TransaccionRollback()
    {
        // Hubo algun error y desahacemos los cambios
        transaccion.Rollback();
        this.conn.Close();
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

    public DataTable traerdata2()
    {
        try
        {
            conectar();
            conn.Open();
            comando.Connection = conn;

            DataTable dtabtrae = new DataTable();
            MySqlDataAdapter adaptrae = new MySqlDataAdapter(comando);
            adaptrae.Fill(dtabtrae);
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
    public DataRow traerfila2()
    {
        try
        {
            conectar();
            conn.Open();
            comando.Connection = conn;
            
            DataTable dtabtrae = new DataTable();
            MySqlDataAdapter adaptrae = new MySqlDataAdapter(comando);
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
        //Modo 1
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
                throw new ApplicationException("Error conectando a base de datos, consulte con su administrador de red", e);            
            return false;
        }
        finally
        {
            conn.Close();
            conn.Dispose();
        }
    }
    public Boolean guardadata2()
    {
        //Modo 2
        try
        {
            conectar();
            conn.Open();
            comando.Connection = conn;
            comando.ExecuteNonQuery();
            comando.Connection.Close();
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
        //Modo 1
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
                throw new ApplicationException("Error conectando a base de datos, consulte con su administrador de red", e);            
            return -1;
        }
        finally
        {
            conn.Close();
            conn.Dispose();
        }
    }
    //Ejecuta una consulta de acción (INSERT, UPDATE, DELETE)
    //sobre la BD, y en caso de AUTO INC me trae el codigo
    public long guardadataid2()
    {
        //Modo 2
        try
        {
            long idgen;
            conectar();
            conn.Open();
            comando.Connection = conn;
            comando.ExecuteNonQuery();
            idgen = comando.LastInsertedId;
            conn.Close();
            comando.Connection.Close();
            return idgen;
        }
        catch (MySqlException e)
        {
            if (e.Number == 1042)            
                throw new ApplicationException("Error conectando a base de datos, consulte con su administrador de red", e);            
            return -1;
        }
        finally
        {
            conn.Close();
            conn.Dispose();
        }
    }
    public int guardadataRecord()
    {
        //Modo 1
        int registrosAlterados = 0;
        try
        {
            conectar();
            conn.Open();
            MySqlCommand cmdguarda = new MySqlCommand(comando.CommandText, conn);
            registrosAlterados = cmdguarda.ExecuteNonQuery();
            cmdguarda.Connection.Close();
            return registrosAlterados;
        }
        catch (MySqlException e)
        {
            if (e.Number == 1042)            
                throw new ApplicationException("Error conectando a base de datos, consulte con su administrador de red", e);            
            return registrosAlterados;
        }
        finally
        {
            conn.Close();
            conn.Dispose();
        }
    }
    public int guardadataRecord2()
    {
        //Modo 2
        int registrosAlterados = 0;
        try
        {
            conectar();
            conn.Open();
            comando.Connection = conn;
            registrosAlterados = comando.ExecuteNonQuery();
            comando.Connection.Close();
            return registrosAlterados;
        }
        catch (MySqlException e)
        {
            if (e.Number == 1042)            
                throw new ApplicationException("Error conectando a base de datos, consulte con su administrador de red", e);            
            return registrosAlterados;
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
    /// Asigna un parámetro de tipo cadena o null al comando creado.
    /// </summary>
    /// <param name="nombre">El nombre del parámetro.</param>
    /// <param name="valor">El valor del parámetro.</param>
    public void AsignarParametroCadenaoNull(string nombre, string valor)
    {
        if (!String.IsNullOrEmpty(valor))
            AsignarParametro(nombre, "'", valor);
        else
            AsignarParametroNulo(nombre);
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

    
    public void AsignarParametroByte(string nombre, byte[] valor)
    {
        comando.Parameters.Add(nombre, MySqlDbType.Blob).Value = valor;
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




    /*Segunda forma*/
    /// <summary>
    /// Setea un parámetro como nulo del comando creado.
    /// </summary>
    /// <param name="nombre">El nombre del parámetro cuyo valor será nulo.</param>
    public void AsignarParametroNulo2(string nombre)
    {
        comando.Parameters.Add(nombre, MySqlDbType.Text).Value = null;
    }

    /// <summary>
    /// Asigna un parámetro de tipo cadena al comando creado.
    /// </summary>
    /// <param name="nombre">El nombre del parámetro.</param>
    /// <param name="valor">El valor del parámetro.</param>
    public void AsignarParametroCadena2(string nombre, string valor)
    {
        comando.Parameters.Add(nombre, MySqlDbType.LongText).Value = valor;
    }
    public void AsignarParametroCadenaoNull2(string nombre, string valor)
    {
        if (!String.IsNullOrEmpty(valor))
            comando.Parameters.Add(nombre, MySqlDbType.LongText).Value = valor;
        else
        {
            comando.Parameters.Add(nombre, null);
            //AsignarParametroNulo(nombre);
        }
        
    }

    public void AsignarParametroVarBinary2(string nombre, byte[] valor)
    {
        comando.Parameters.Add(nombre, MySqlDbType.VarBinary).Value = valor;
    }

    /// <summary>
    /// Asigna un parámetro de tipo entero al comando creado.
    /// </summary>
    /// <param name="nombre">El nombre del parámetro.</param>
    /// <param name="valor">El valor del parámetro.</param>
    public void AsignarParametroEntero2(string nombre, int valor)
    {
        comando.Parameters.Add(nombre, MySqlDbType.Int64).Value = valor;
    }


    /// <summary>
    /// Asigna un parámetro de tipo doble al comando creado.
    /// </summary>
    /// <param name="nombre">El nombre del parámetro.</param>
    /// <param name="valor">El valor del parámetro.</param>
    public void AsignarParametroDouble2(string nombre, double valor)
    {
        comando.Parameters.Add(nombre, MySqlDbType.Double).Value = valor;
    }

    /// <summary>
    /// Asigna un parámetro de tipo fecha al comando creado.
    /// </summary>
    /// <param name="nombre">El nombre del parámetro.</param>
    /// <param name="valor">El valor del parámetro.</param>
    public void AsignarParametroFecha2(string nombre, DateTime valor)
    {
        comando.Parameters.Add(nombre, MySqlDbType.DateTime).Value = valor;
    }
    // Metodos se crear para la transacciones
    public Boolean guardadataTransaccion(ConexionMultiple conexion)
    {
        try
        {
            comando.Connection = conexion.conn;
            comando.ExecuteNonQuery();
            comando.Parameters.Clear();
            return true;
        }
        catch (MySqlException e)
        {

            if (e.Number == 1042)
            {
                throw new ApplicationException("Error conectando a base de datos, consulte con su administrador de red", e);
            }
            if (e.Number == 1062)
            {
                throw new ApplicationException("Existen llaves duplicadas", e);
            }

            return false;
        }
    }
    public long guardadataIdTransaccion(ConexionMultiple conexion)
    {
        //Modo 2
        try
        {
            long idgen;
            comando.Connection = conexion.conn;
            comando.ExecuteNonQuery();
            idgen = comando.LastInsertedId;
            comando.Parameters.Clear();
            return idgen;
        }
        catch (MySqlException e)
        {
            if (e.Number == 1042)
                throw new ApplicationException("Error conectando a base de datos, consulte con su administrador de red", e);
            return -1;
        }
    }
    public int guardadataRecordTransaccion(ConexionMultiple conexion)
    {
        //Modo 2
        try
        {
            int registrosAlterados = 0;
            //long idgen;
            comando.Connection = conexion.conn;
            registrosAlterados = comando.ExecuteNonQuery();           
            comando.Parameters.Clear();
            return registrosAlterados;
        }
        catch (MySqlException e)
        {
            if (e.Number == 1042)
                throw new ApplicationException("Error conectando a base de datos, consulte con su administrador de red", e);
            return -1;
        }
    }
}
