using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace siacademic
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_PreInit(Object sender, EventArgs e)
        {
            if (Session["codperfil"] == null && Session["plataforma"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            //if (Funciones.nombreCadena2() == "instenalco" || Funciones.nombreCadena2() == "meiradelmar" || Funciones.nombreCadena2() == "soniaahumada" || Funciones.nombreCadena2() == "nuevagranada" || Funciones.nombreCadena2() == "noroccidentalsoledad" || Funciones.nombreCadena2() == "tecvistahermosa" || Funciones.nombreCadena2() == "jorgenabello" || Funciones.nombreCadena2() == "betanianorte" || Funciones.nombreCadena2() == "tecmetropolitano" || Funciones.nombreCadena2() == "teccomercialsabanalarga")
            //{
            //    Response.Redirect("https://sian365.co/");
            //}
            //if (Funciones.nombreCadena() == "nuevagranada")
            //{
            //    Response.Redirect("https://controlacademic.co/nuevagranada");
            //}
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["plataforma"] != null)
            {
                switch (Session["plataforma"].ToString())
                {
                    case "ca":
                        Page.Title = Page.Title + "  Control Academic";
                        ltIco.Text = "<link href='Imagenes/favicon.ico' rel='shortcut icon' type='image/x-icon' />";
                        break;
                    case "cak":
                        Page.Title = Page.Title + "  Control Academic Kids";
                        ltIco.Text = "<link href='Imagenes/faviconca.ico' rel='shortcut icon' type='image/x-icon' />";
                        break;
                    case "s":
                        Page.Title = Page.Title + "  SIAN365";
                        ltIco.Text = "<link href='Imagenes/favicons.ico' rel='shortcut icon' type='image/x-icon' />";
                        imgXNoti.Attributes.Add("style", "font-size: 15px; color:#776E4A;");
                        imgXMens.Attributes.Add("style", "font-size: 15px; color:#776E4A;");
                        break;
                }
            }
            else
            {
                Response.Redirect("Default.aspx");
            }

            if (!IsPostBack)
            {
                switch (Funciones.nombreCadena())
                {
                    case "sanjose":
                        logoMiniColegio.ImageUrl = "https://controlacademic.co/sanjose/Imagenes/login/sanjose/logo-mini.png";
                        logoMiniColegio.Visible = true;
                        break;
                    case "grupolaureles":
                        logoMiniColegio.ImageUrl = "https://cloudtechnologys.co/ImagenesPublicas/laurel-moneda.png";
                        logoMiniColegio.Visible = true;
                        break;
                }

                lblP.Text = Session["plataforma"].ToString();
                if (Session["codperfil"] != null && Session["plataforma"] != null)
                {
                    //Usuarios user = new Usuarios();
                    ius.Value = Session["coduser"].ToString();
                    lblNameUsuarioPerfil.InnerHtml = "<a class='perfil-user-link'>" + Session["nombre"].ToString() + "</a>";
                    //lblRegistrado.Text = "Bienvenido: <b>" + Session["nombre"].ToString() + "</b>";

                    if (Session["listadoMenu"] == null)
                    {
                        Session["listadoMenu"] = cargarMenu2();
                        cssmenu.InnerHtml = Session["listadoMenu"].ToString();
                    }
                    else
                        cssmenu.InnerHtml = Session["listadoMenu"].ToString();

                    inhabilitarMenuIzq();

                    try
                    {
                        if (Session["imagePerfilSRC"] == null)
                            buscarImagenUsuario();
                        else
                            imgPerfilGeneral.ImageUrl = Session["imagePerfilSRC"].ToString();
                    }
                    catch (Exception) { }
                    //if (Session["codtipousuario"].ToString() == "1")
                    //{
                    //    PanelAyuda.Visible = true;
                    //}
                    //else
                    //{
                    //    PanelAyuda.Visible = false;
                    //}


                }
                else
                {
                    Response.Redirect("Default.aspx");
                }
            }
            else
            {
                //string nombreCadena = Funciones.nombreCadena();
                ////nombreCadena = "cis";

                ///*Si es el CIS, y es un papa o estudiante volvemos a cargar el menu completo, porque el item
                // de aula virtual depende del grado del estudiante seleccionado cuando es un papa*/
                //if (nombreCadena == "cis" && ((Session["codtipousuario"].ToString() == "2") || (Session["codtipousuario"].ToString() == "6")))
                //{
                //    Session["listadoMenu"] = cargarMenu2();
                //    cssmenu.InnerHtml = Session["listadoMenu"].ToString();
                //}
            }
        }

        public string linkCuadroHonor2()
        {
            string url = "";
            if (Session["token_laravel"] != null && Session["token_refresh_laravel"] != null)
            {
                string nombrecolegio = Funciones.nombreCadena();
                string urlHost = System.Web.HttpContext.Current.Request.Url.Host;
                if (urlHost == "localhost")
                    url = "http://" + urlHost + ":4200/login/user?token=" + Session["token_laravel"].ToString() + "&refresh=" + Session["token_refresh_laravel"].ToString() + "&redirect=honor/reconocimiento";
                else
                    url = "https://" + urlHost + "/web" + nombrecolegio + "/login/user?token=" + Session["token_laravel"].ToString() + "&refresh=" + Session["token_refresh_laravel"].ToString() + "&redirect=honor/reconocimiento";
            }
            return url;
        }
        /// <summary>
        /// Depende de la Session["cambioPerfil"]
        /// </summary>
        public void inhabilitarMenuIzq()
        {
            if (Session["cambioPerfil"] != null)
            {
                menuizq.Attributes.Add("style", "display:none");
                contenido.Attributes.Add("style", "margin: 30px 0px 0px 0px;");
            }
            else
            {
                menuizq.Attributes.Add("style", "display:block");
                //contenido.Attributes.Add("style", "margin: 30px 0px 0px 50px;");
            }
        }
        private string cargarMenu2()
        {
            string linkpagina = Request.Url.Segments[Request.Url.Segments.Length - 1];
            string cadena = "";
            string codmenu = ""; string nombremenu = ""; string link = ""; string target = "";
            Menu men = new Menu();
            List<menu> listaMenus = men.cargarMenuV2(Session["codperfil"].ToString());
            if (listaMenus != null)
            {
                cadena += "<ul>";
                var nivel1 = (from i in listaMenus
                              where i.nivel == "1"
                              select i).ToList();
                foreach (menu menu in nivel1)
                {
                    codmenu = menu.cod;
                    nombremenu = menu.nombre;
                    link = menu.link;

                    var submenu = from j in listaMenus
                                  where j.codsuperior == menu.cod
                                  select j;

                    if (submenu.FirstOrDefault() != null)
                    {
                        //Tienes Submesnu
                        string activo = "no";
                        foreach (menu itemsubmenu in submenu)
                        {
                            if (itemsubmenu.link == linkpagina || itemsubmenu.link == linkpagina + ".aspx")
                            {
                                activo = "si";
                            }
                        }

                        if (activo == "no")
                        {
                            cadena += "<li class='has-sub'><a href='" + link + "'><span><img class='imgMenu' src='" + menu.imagen + "' /><span class='textoLinkMenu'>" +
                                nombremenu + "</span><i class='fa fa-angle-left pull-right'></i></span></a>";
                            cadena += "<ul>";
                        }
                        else
                        {
                            cadena += "<li class='has-sub active'><a href='" + link + "'><span><img class='imgMenu' src='" + menu.imagen + "' /><span class='textoLinkMenu'>" +
                                nombremenu + "</span><i class='fa fa-angle-left pull-right'></i></span></a>";
                            cadena += "<ul style='display:block;'>";
                        }

                        foreach (menu itemsubmenu in submenu)
                        {
                            codmenu = itemsubmenu.cod;
                            nombremenu = itemsubmenu.nombre;
                            link = itemsubmenu.link;
                            target = itemsubmenu.target;

                            if (itemsubmenu.link == linkpagina || itemsubmenu.link == linkpagina + ".aspx")
                                cadena += "<li><a class='activo' target='" + target + "' href='" + link + "'><span>" + nombremenu + "</span></a></li>";
                            else
                                cadena += "<li ><a class='inactivo' target='" + target + "' href='" + link + "'><span>" + nombremenu + "</span></a></li>";
                        }

                        cadena += "</ul>";
                    }
                    else
                    {
                        string nombreCadena = Funciones.nombreCadena();
                        //nombreCadena = "cis";
                        //Validamos si es un estudiante que NO es de preescolar del colegio Cartagena International School y si el link del aula virtual
                        if (nombreCadena == "cis" && (menu.link == "aulavirtualest2" || menu.link == "aulavirtualest2.aspx")
                            && ((Session["codtipousuario"].ToString() == "2") || (Session["codtipousuario"].ToString() == "6")))
                        {
                            if (validarEstudiantePreescolarCIS())
                                cadena += "<li class='last'><a target='" + menu.target + "' title='" + menu.alt + "' href='" + link +
                                     "'><span><img class='imgMenu' src='" + menu.imagen + "' /><span class='textoLinkMenu'>" + nombremenu + "</span></span></a>";

                        }
                        else//Sino es es el CIS, aulavirtualest2.aspx, papá o estudiante seguimos normal
                            //Este no tiene submenus
                            cadena += "<li class='last'><a target='" + menu.target + "' title='" + menu.alt + "' href='" + link +
                                "'><span><img class='imgMenu' src='" + menu.imagen + "' /><span class='textoLinkMenu'>" + nombremenu + "</span></span></a>";

                    }

                }//Fin del ForEach Menus
                cadena += "</ul>";
            }
            return cadena;
        }
        private bool validarEstudiantePreescolarCIS()
        {
            //bool estudiantePreescolarCIS = false;
            //if (Session["codestudiante"] != null && Session["codestudiante"].ToString() != "") //Ahora validamos si el estudiante o Padre Logueado Actualmente esta en el nivel de preEscolar
            //{
            //    string codestudiante = Session["codestudiante"].ToString();
            //    if (Matricula.validarEstudiantePreescolar(codestudiante))//Si es de preescolar le mostramos el menu
            //        estudiantePreescolarCIS = true;
            //    else // Sino no le mostramos el menu
            //        estudiantePreescolarCIS = false;
            //}
            //else
            //    estudiantePreescolarCIS = true;

            //return estudiantePreescolarCIS;

            bool estudiantePreescolarCIS = false;
            if (Session["codestumatricula"] != null && Session["codestumatricula"].ToString() != "") //Ahora validamos si el estudiante o Padre Logueado Actualmente esta en el nivel de preEscolar
            {
                string codestumatricula = Session["codestumatricula"].ToString();
                if (validarEstudiantePreescolarNuevo(codestumatricula))//Si es de preescolar le mostramos el menu
                    estudiantePreescolarCIS = true;
                else // Sino no le mostramos el menu
                    estudiantePreescolarCIS = false;
            }
            else
                estudiantePreescolarCIS = true;

            return estudiantePreescolarCIS;
        }
        public bool validarEstudiantePreescolarNuevo(string codestumatricula)
        {
            bool siEstaMatriculado = false;
            DataRow datoMatricula = validarEstudiantePreescolarConsultaNuevo(codestumatricula);
            if (datoMatricula != null)//Si es distinto de NULL, Quiere decir que el estudiantes si esta matricula en el nivel de Preescolar
            {
                siEstaMatriculado = true;
            }
            return siEstaMatriculado;
        }
        public static DataRow validarEstudiantePreescolarConsultaNuevo(string codestumatricula)
        {
            Conexion conector = new Conexion();
            String consulta = "SELECT * FROM `aca_estumatricula` em INNER JOIN `aca_gradoscursos` gc ON em.`codgradoscursos`=gc.`cod` INNER JOIN `aca_grados` g ON gc.`codgrado`=g.`cod` INNER JOIN `aca_niveledu` ne ON g.`codniveledu`=ne.`cod` INNER JOIN `con_aniolectivo` al ON em.`codanio`=al.`cod` WHERE em.`cod`=@codestumatricula AND ne.`codtipovaloracion`='2'";
            conector.CrearComando(consulta);
            conector.AsignarParametroCadena("@codestumatricula", codestumatricula);
            return conector.traerfila();
        }

        private string cargarMenu()
        {
            string linkpagina = Request.Url.Segments[Request.Url.Segments.Length - 1];
            string cadena = "";
            string codmenu = ""; string nombremenu = ""; string link = ""; string target = "";
            Menu men = new Menu();
            DataTable datos = men.cargarMenuSuperior(Session["codperfil"].ToString());
            if (datos != null && datos.Rows.Count > 0)
            {
                cadena += "<ul>";
                for (int i = 0; i < datos.Rows.Count; i++)
                {
                    codmenu = datos.Rows[i]["cod"].ToString();
                    nombremenu = datos.Rows[i]["nombre"].ToString();
                    link = datos.Rows[i]["link"].ToString();
                    DataTable datos2 = men.cargarMenu2Nivel(codmenu, Session["codperfil"].ToString());
                    if (datos2 != null && datos2.Rows.Count > 0)//Si tiene submenu
                    {
                        string activo = "no";
                        for (int j = 0; j < datos2.Rows.Count; j++)
                        {
                            if (datos2.Rows[j]["link"].ToString() == linkpagina || datos2.Rows[j]["link"].ToString() == linkpagina + ".aspx")
                            {
                                activo = "si";
                            }
                        }

                        if (activo == "no")
                        {
                            cadena += "<li class='has-sub'><a href='" + link + "'><span><img class='imgMenu' src='" + datos.Rows[i]["imagen"].ToString() + "' />" + nombremenu + "</span></a>";
                            cadena += "<ul>";
                        }
                        else
                        {
                            cadena += "<li class='has-sub active'><a href='" + link + "'><span><img class='imgMenu' src='" + datos.Rows[i]["imagen"].ToString() + "' />" + nombremenu + "</span></a>";
                            cadena += "<ul style='display:block;'>";
                        }

                        for (int j = 0; j < datos2.Rows.Count; j++)
                        {
                            codmenu = datos2.Rows[j]["cod"].ToString();
                            nombremenu = datos2.Rows[j]["nombre"].ToString();
                            link = datos2.Rows[j]["link"].ToString();
                            target = datos2.Rows[j]["target"].ToString();
                            if (datos2.Rows[j]["link"].ToString() == linkpagina || datos2.Rows[j]["link"].ToString() == linkpagina + ".aspx")
                                cadena += "<li><a class='activo' target='" + target + "' href='" + link + "'><span>" + nombremenu + "</span></a></li>";
                            else
                                cadena += "<li ><a class='inactivo' target='" + target + "' href='" + link + "'><span>" + nombremenu + "</span></a></li>";

                        }
                        cadena += "</ul>";
                    }
                    else
                    {
                        cadena += "<li class='last'><a target='" + datos.Rows[i]["target"].ToString() + "' title='" + datos.Rows[i]["alt"].ToString() + "' href='" + link + "'><span><img class='imgMenu' src='" + datos.Rows[i]["imagen"].ToString() + "' />" + nombremenu + "</span></a>";
                    }
                }
                cadena += "</ul>";
            }
            return cadena;
        }
        protected void btnSesion_Click(object sender, EventArgs e)
        {
            Session.RemoveAll();
            Response.Redirect("default.aspx");
        }
        private void buscarImagenUsuario()
        {
            Usuarios usu = new Usuarios();
            DataRow dr = usu.buscarImagenUsuario(Session["coduser"].ToString());
            if (dr != null)
            {
                imgPerfilGeneral.ImageUrl = dr["path"].ToString();
                Session["imagePerfilSRC"] = dr["path"].ToString();
            }
            else
            {
                imgPerfilGeneral.ImageUrl = "~/Imagenes/img-default.png";
                Session["imagePerfilSRC"] = "~/Imagenes/img-default.png";
            }
        }
        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {

            if (menuizq.Attributes["class"].Equals("cm-sidebar"))
            {
                menuizq.Attributes.Add("class", "leftMenu2");
                contenido.Attributes.Add("class", "content content-sin-menu");
            }
            else
            {
                menuizq.Attributes.Add("class", "cm-sidebar");
                contenido.Attributes.Add("class", "content content-con-menu");
            }

        }
        public void cargarHijosdelFamiliar()
        {
            Configuraciones con = new Configuraciones();

            //DataTable datosEstFam = new Estudiantes().cargarHijosdelFamiliar(Session["identificacion"].ToString());
            DataTable datosEstFam = cargarHijosdelFamiliarNuevo(Session["identificacion"].ToString());
            if (datosEstFam != null && datosEstFam.Rows.Count > 0)
            {
                DataRow[] datosEstActivosFam = datosEstFam.Select("cgc IS NOT NULL");//solamente cargamos estudiantes que estén activos (matriculados en el año actual)
                if (datosEstActivosFam != null && datosEstActivosFam.Length > 0)
                {
                    string ca = "";
                    ca += "<div>";
                    ca += "<div class='tituloSeleccionDeHijos'>";
                    ca += "<div style='width:250px; overflow: hidden; text-overflow: ellipsis; position:absolute;'>";
                    ca += "<span class='pull-left' style='font-size:12px; margin-left:4px; margin-top:2px; white-space:nowrap;'>Listado de estudiantes</span>";
                    ca += "</div>";
                    ca += "<div id='minimizarHijo' class='minimizarHijo pull-right' title='Minimizar'>";
                    ca += "<i class='fa fa-minus' style='font-size:14px;'></i>";
                    ca += "</div>";
                    ca += "</div>";
                    ca += "<div class='seleccionDeHijos'>";

                    DataRow datogen = con.cargarEleccionesConfiguraciones();

                    #region ANTES
                    //una validación antes
                    //if (Session["codestudiante"] != null)
                    //{
                    //    int encontroUsuarioEstudiante = 0;
                    //    for (int i = 0; i < datosEstActivosFam.Length; i++)
                    //    {
                    //        string codestudiante = datosEstActivosFam[i]["codestudiante"].ToString();
                    //        if (codestudiante == Session["codestudiante"].ToString())
                    //        {
                    //            encontroUsuarioEstudiante = 1;
                    //        }
                    //    }

                    //    if (encontroUsuarioEstudiante == 0)
                    //        Session["codestudiante"] = datosEstActivosFam[0]["codestudiante"].ToString();
                    //}
                    #endregion

                    if (Session["codestumatricula"] != null)
                    {
                        int encontroUsuarioEstudiante = 0;
                        for (int i = 0; i < datosEstActivosFam.Length; i++)
                        {
                            string codestumatricula = datosEstActivosFam[i]["codestumatricula"].ToString();
                            if (codestumatricula == Session["codestumatricula"].ToString())
                            {
                                encontroUsuarioEstudiante = 1;
                            }
                        }

                        if (encontroUsuarioEstudiante == 0)
                            Session["codestumatricula"] = datosEstActivosFam[0]["codestumatricula"].ToString();
                    }

                    for (int i = 0; i < datosEstActivosFam.Length; i++)
                    {
                        string codestudiante = datosEstActivosFam[i]["codestudiante"].ToString();
                        string anio = datosEstActivosFam[i]["anio"].ToString();
                        string codestumatricula = datosEstActivosFam[i]["codestumatricula"].ToString();
                        string nombrecompletoestudiante = datosEstActivosFam[i]["nombrecompletoestudiante"].ToString();
                        string descripcioncurso = datosEstActivosFam[i]["descripcioncurso"].ToString();

                        if (datogen != null)
                            if (datogen["mostrarsologradolistadohijo"].ToString() == "1")
                                descripcioncurso = datosEstActivosFam[i]["descripciongrado"].ToString();

                        string pathimgperfilest = "";
                        if (datosEstActivosFam[i]["pathimgestudiante"].ToString().Trim() != "")
                            pathimgperfilest = datosEstActivosFam[i]["pathimgestudiante"].ToString().Remove(0, 2);
                        else
                            pathimgperfilest = "Imagenes/img-default.png";

                        string hijoActivo = "";
                        string imgHijoActivo = "";
                        #region ANTES
                        //if (Session["codestudiante"] == null)
                        //{
                        //    if (i == 0)
                        //    {
                        //        Session["codestudiante"] = codestudiante;
                        //        hijoActivo = "itemHijoActivo";
                        //        imgHijoActivo += "<div style='position:relative;'>";
                        //        imgHijoActivo += "<div class='pull-right' style='position: absolute; right: 0px; bottom: -5px;'>";
                        //        imgHijoActivo += "<img src='Imagenes/ok.png' style='width: 30px;'/>";
                        //        imgHijoActivo += "</div>";
                        //        imgHijoActivo += "</div>";
                        //    }
                        //}
                        //else
                        //{
                        //    if (codestudiante == Session["codestudiante"].ToString())
                        //    {
                        //        hijoActivo = "itemHijoActivo";

                        //        imgHijoActivo += "<div style='position:relative;'>";
                        //        imgHijoActivo += "<div class='pull-right' style='position: absolute; right: 0px; bottom: -5px;'>";
                        //        imgHijoActivo += "<img src='Imagenes/ok.png' style='width: 30px;'/>";
                        //        imgHijoActivo += "</div>";
                        //        imgHijoActivo += "</div>";
                        //    }
                        //}
                        #endregion

                        if (Session["codestumatricula"] == null)
                        {
                            if (i == 0)
                            {
                                Session["codestumatricula"] = codestumatricula;

                                hijoActivo = "itemHijoActivo";
                                imgHijoActivo += "<div style='position:relative;'>";
                                imgHijoActivo += "<div class='pull-right' style='position: absolute; right: 0px; bottom: -5px;'>";
                                imgHijoActivo += "<img src='Imagenes/ok.png' style='width: 30px;'/>";
                                imgHijoActivo += "</div>";
                                imgHijoActivo += "</div>";
                            }
                        }
                        else
                        {
                            if (codestumatricula == Session["codestumatricula"].ToString())
                            {
                                hijoActivo = "itemHijoActivo";
                                imgHijoActivo += "<div style='position:relative;'>";
                                imgHijoActivo += "<div class='pull-right' style='position: absolute; right: 0px; bottom: -5px;'>";
                                imgHijoActivo += "<img src='Imagenes/ok.png' style='width: 30px;'/>";
                                imgHijoActivo += "</div>";
                                imgHijoActivo += "</div>";
                            }
                        }

                        ca += "<div class='itemHijo " + hijoActivo + "' onclick='cambiarHijoSeleccionado(" + codestumatricula + ")'>";
                        ca += "<div class='avatarHijo'>";

                        bool sw = false;
                        if (datogen != null)
                            if (datogen["mostrargradolistadohijo"].ToString() != "1")
                                sw = true;

                        ca += "<img src='" + pathimgperfilest + "' alt='' style='width: 100%; border-radius: 10%;-moz-border-radius: 10%'/>";
                        ca += "</div>";
                        ca += "<div class='descripcionHijo'>";
                        ca += "    <span style='font-size:14px;font-weight:500;white-space:nowrap;' class='tooltipClase' title='" + nombrecompletoestudiante + "'>" + nombrecompletoestudiante + "</span>";
                        ca += "<br />" + (sw ? "<br />" : "");

                        if (!sw)
                            ca += "<span style='font-size:12px;'>(" + descripcioncurso + " - " + anio + ")</span>";

                        ca += imgHijoActivo;

                        ca += "</div>";
                        ca += "</div>";
                    }
                    ca += "</div>";//</div class='seleccionDeHijos'>
                    ca += "</div>";

                    divSelecciondeHijos.InnerHtml = ca;
                    if (Funciones.nombreCadena() == "cis")
                        cssmenu.InnerHtml = cargarMenu2();
                }
            }
        }
        public DataTable cargarHijosdelFamiliarNuevo(string idfamiliar)
        {
            Conexion conector = new Conexion();
            string consulta = "SELECT *, (SELECT codigoespecial FROM usu_codigoespecial WHERE codusuario = u.cod) 'codigoespecial', u.cod 'codusuarioest', CONCAT_WS ( ' ', e.`primernombre`, e.`segundonombre`, e.`primerapellido`, e.`segundoapellido` ) 'nombrecompletoestudiante', g.codniveledu, em.codanio, em.codgradoscursos 'cgc', em.cod 'codestumatricula', gc.descripcion 'descripcioncurso', g.nombre 'descripciongrado', (SELECT path FROM usu_imagenperfil uip INNER JOIN usu_usuario u ON uip.codusuario = u.cod WHERE u.identificacion = e.codigo AND u.codtipousuario = '2' LIMIT 1) 'pathimgestudiante', (SELECT codanio FROM adm_procesoestudiante WHERE codestudiante = e.codigo LIMIT 1) 'codanioadm' FROM aca_estumatricula em INNER JOIN con_aniolectivo al ON al.cod = em.`codanio` INNER JOIN aca_gradoscursos gc ON gc.cod = em.codgradoscursos INNER JOIN aca_grados g ON g.cod = gc.codgrado INNER JOIN est_familiareshijos fh ON fh.`codestudiante` = em.`codestudiante` INNER JOIN est_familiares f ON fh.`codfamiliar` = f.cod INNER JOIN est_estudiantes e ON fh.`codestudiante` = e.codigo LEFT JOIN usu_usuario u ON u.identificacion = e.codigo AND u.codtipousuario = '2' WHERE f.id = @idfamiliar AND al.cod IN (SELECT al.cod FROM con_aniolectivo al INNER JOIN aca_periodossemestre ps ON ps.codanio=al.cod WHERE IF(ps.`codniveledu` IS NULL, 1=1, ps.`codniveledu` = g.`codniveledu`) AND DATE(NOW()) BETWEEN ps.fechainicio AND ps.`fechafin` GROUP BY al.cod)";
            conector.CrearComando(consulta);
            conector.AsignarParametroCadena("@idfamiliar", idfamiliar);
            return conector.traerdata();
        }
    }
}