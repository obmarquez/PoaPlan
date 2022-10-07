using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoaPlan.Data;
using PoaPlan.Helper;
using PoaPlan.Models;
using PoaPlan.Models.Consultas;

namespace PoaPlan.Controllers
{
    [Authorize]

    [Authorize(Roles = "Administrador, Director, Enlace")]
    public class ImpresionesController : Controller
    {
        float[] widthsTitulosGenerales = new float[] { 1f };
        private DBOperaciones repo;

        public ImpresionesController()
        {
            repo = new DBOperaciones();
        }
        
        public IActionResult PoaMensual(int anio = 0, string elMesesillo = "")
        {
            ViewBag.losMeses = Constantes.Meses();

            if (anio == 0)
            {
                return View();
            }
            else
            {
                ViewBag.elMes = elMesesillo;
                return View(repo.Getdosparam1<ListaOrganos>("sp_organos_por_anio", new { @anio = anio}).ToList());
            }            
        }

        [Authorize(Roles = "Administrador, Director, Enlace")]
        public IActionResult PrtPoaMes(string elMes, int elArea)
        {
            //obtención del organo administrativo
            var datosOrgano = repo.Getdosparam1<ClavePresupuestaria>("obtenerOrgano", new { @idArea = elArea }).FirstOrDefault();
            //obtencion componente, indicador, programado y avance por organo
            var datosOrganoComponente = repo.Getdosparam1<ImpresionAvanceMensual>("sp_impresion_componente_indicador_totPro_totAva_avaMensual", new { @idArea = elArea }).ToList();

            MemoryStream msRep = new MemoryStream();

            //Document docPoaMes = new Document(PageSize.LETTER, 30f, 20f, 70f, 40f);
            Document docPoaMes = new Document(PageSize.LETTER.Rotate(), 30f, 20f, 70f, 40f);

            PdfWriter pwRep = PdfWriter.GetInstance(docPoaMes, msRep);

            string elTitulo = "Programa Operativo Anual 2022";
            string elLider = datosOrgano.lider;

            pwRep.PageEvent = HeaderFooterPoa.getMultilineFooter(elTitulo, elLider);

            docPoaMes.Open();

            var fonEtiqueta = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK);
            var fonDato = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK);

            var fonEtiqueta_b = FontFactory.GetFont("Arial", 7, Font.NORMAL, BaseColor.BLACK);
            var fonDato_b = FontFactory.GetFont("Arial", 7, Font.BOLD, BaseColor.BLACK);

            #region Datos organo
            PdfPTable tblOrgano = new PdfPTable(2)
            {
                TotalWidth = 730,
                LockedWidth = true
            };

            float[] values = new float[2];
            values[0] = 150;
            values[1] = 580;
            tblOrgano.SetWidths(values);
            tblOrgano.HorizontalAlignment = 0;
            tblOrgano.SpacingAfter = 5f;
            tblOrgano.SpacingBefore = 5f;
            tblOrgano.DefaultCell.Border = 0;

            PdfPCell celCvePre = new PdfPCell(new Phrase("Órgano Administrativo", fonEtiqueta));
            celCvePre.BorderWidth = 0;
            celCvePre.HorizontalAlignment = Element.ALIGN_LEFT;

            PdfPCell celDatCve = new PdfPCell(new Phrase(datosOrgano.organo, fonDato));
            celDatCve.BorderWidth = 0;
            celDatCve.BorderWidthBottom = 1;
            celDatCve.HorizontalAlignment = Element.ALIGN_CENTER;

            tblOrgano.AddCell(celCvePre);
            tblOrgano.AddCell(celDatCve);

            docPoaMes.Add(tblOrgano);
            #endregion

            #region Impresion del mes
            Paragraph elMestransmitido = new Paragraph(new Phrase("Mes del informe: " + elMes, fonEtiqueta_b));
            elMestransmitido.Alignment = Element.ALIGN_RIGHT;

            docPoaMes.Add(elMestransmitido);
            #endregion

            #region ciclo Componente
            foreach (var item in datosOrganoComponente)
            {
                int valormensual = 0;
                int valormensualprogramado = 0;

                #region Titulo componente
                PdfPTable titulocomponente = new PdfPTable(1)
                {
                    TotalWidth = 730,
                    LockedWidth = true
                };

                titulocomponente.SetWidths(widthsTitulosGenerales);
                titulocomponente.HorizontalAlignment = 0;
                //titulocomponente.SpacingAfter = 10f;
                titulocomponente.SpacingBefore = 10f;

                PdfPCell cellTituloComponente = new PdfPCell(new Phrase("COMPONENTE", fonDato));
                cellTituloComponente.HorizontalAlignment = Element.ALIGN_CENTER;
                cellTituloComponente.BackgroundColor = new iTextSharp.text.BaseColor(234, 236, 238);
                cellTituloComponente.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
                cellTituloComponente.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;
                titulocomponente.AddCell(cellTituloComponente);

                docPoaMes.Add(titulocomponente);
                #endregion

                #region dentroComponente
                PdfPTable intitulocomponente = new PdfPTable(8)
                {
                    TotalWidth = 730,
                    LockedWidth = true
                };

                float[] valuesInComponente = new float[8];
                valuesInComponente[0] = 215;
                valuesInComponente[1] = 215;
                valuesInComponente[2] = 50;
                valuesInComponente[3] = 50;
                valuesInComponente[4] = 50;
                valuesInComponente[5] = 50;
                valuesInComponente[6] = 50;
                valuesInComponente[7] = 50;

                intitulocomponente.SetWidths(valuesInComponente);
                intitulocomponente.HorizontalAlignment = 0;
                //intitulocomponente.SpacingAfter = 10f;
                //intitulocomponente.SpacingBefore = 10f;

                PdfPCell cellResCom = new PdfPCell(new Phrase("Resumen Narrativo", fonDato_b));
                cellResCom.HorizontalAlignment = Element.ALIGN_CENTER;
                cellResCom.VerticalAlignment = Element.ALIGN_MIDDLE;
                cellResCom.UseAscender = true;
                cellResCom.BackgroundColor = new iTextSharp.text.BaseColor(234, 236, 238);
                cellResCom.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
                cellResCom.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;

                PdfPCell cellindCom = new PdfPCell(new Phrase("Indicador", fonDato_b));
                cellindCom.HorizontalAlignment = Element.ALIGN_CENTER;
                cellindCom.VerticalAlignment = Element.ALIGN_MIDDLE;
                cellindCom.UseAscender = true;
                cellindCom.BackgroundColor = new iTextSharp.text.BaseColor(234, 236, 238);
                cellindCom.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
                cellindCom.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;

                PdfPCell cellProMen = new PdfPCell(new Phrase("Programado Mensual", fonDato_b));
                cellProMen.HorizontalAlignment = Element.ALIGN_CENTER;
                cellProMen.VerticalAlignment = Element.ALIGN_MIDDLE;
                cellProMen.UseAscender = true;
                cellProMen.BackgroundColor = new iTextSharp.text.BaseColor(234, 236, 238);
                cellProMen.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
                cellProMen.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;

                PdfPCell cellAvaMen = new PdfPCell(new Phrase("Avance Mensual", fonDato_b));
                cellAvaMen.HorizontalAlignment = Element.ALIGN_CENTER;
                cellAvaMen.VerticalAlignment = Element.ALIGN_MIDDLE;
                cellAvaMen.UseAscender = true;
                cellAvaMen.BackgroundColor = new iTextSharp.text.BaseColor(234, 236, 238);
                cellAvaMen.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
                cellAvaMen.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;

                PdfPCell cellPorMen = new PdfPCell(new Phrase("% Mensual", fonDato_b));
                cellPorMen.HorizontalAlignment = Element.ALIGN_CENTER;
                cellPorMen.VerticalAlignment = Element.ALIGN_MIDDLE;
                cellPorMen.UseAscender = true;
                cellPorMen.BackgroundColor = new iTextSharp.text.BaseColor(234, 236, 238);
                cellPorMen.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
                cellPorMen.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;

                PdfPCell cellMeta = new PdfPCell(new Phrase("Meta Anual Programada", fonDato_b));
                cellMeta.HorizontalAlignment = Element.ALIGN_CENTER;
                cellMeta.VerticalAlignment = Element.ALIGN_MIDDLE;
                cellMeta.UseAscender = true;
                cellMeta.BackgroundColor = new iTextSharp.text.BaseColor(234, 236, 238);
                cellMeta.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
                cellMeta.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;

                PdfPCell cellAcu = new PdfPCell(new Phrase("Meta Acumulada", fonDato_b));
                cellAcu.HorizontalAlignment = Element.ALIGN_CENTER;
                cellAcu.VerticalAlignment = Element.ALIGN_MIDDLE;
                cellAcu.UseAscender = true;
                cellAcu.BackgroundColor = new iTextSharp.text.BaseColor(234, 236, 238);
                cellAcu.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
                cellAcu.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;

                PdfPCell cellPor = new PdfPCell(new Phrase("% Avance Físico", fonDato_b));
                cellPor.HorizontalAlignment = Element.ALIGN_CENTER;
                cellPor.VerticalAlignment = Element.ALIGN_MIDDLE;
                cellPor.UseAscender = true;
                cellPor.BackgroundColor = new iTextSharp.text.BaseColor(234, 236, 238);
                cellPor.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
                cellPor.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;

                intitulocomponente.AddCell(cellResCom);
                intitulocomponente.AddCell(cellindCom);
                intitulocomponente.AddCell(cellProMen);
                intitulocomponente.AddCell(cellAvaMen);
                intitulocomponente.AddCell(cellPorMen);
                intitulocomponente.AddCell(cellMeta);
                intitulocomponente.AddCell(cellAcu);
                intitulocomponente.AddCell(cellPor);

                docPoaMes.Add(intitulocomponente);
                #endregion

                #region ValdentroComponente
                PdfPTable Valintitulocomponente = new PdfPTable(8)
                {
                    TotalWidth = 730,
                    LockedWidth = true
                };

                float[] ValvaluesInComponente = new float[8];
                ValvaluesInComponente[0] = 215;
                ValvaluesInComponente[1] = 215;
                ValvaluesInComponente[2] = 50;
                ValvaluesInComponente[3] = 50;
                ValvaluesInComponente[4] = 50;
                ValvaluesInComponente[5] = 50;
                ValvaluesInComponente[6] = 50;
                ValvaluesInComponente[7] = 50;

                Valintitulocomponente.SetWidths(ValvaluesInComponente);
                Valintitulocomponente.HorizontalAlignment = 0;
                Valintitulocomponente.SpacingAfter = 10f;
                //intitulocomponente.SpacingBefore = 10f;

                PdfPCell ValcellResCom = new PdfPCell(new Phrase(item.componente, fonEtiqueta_b));
                ValcellResCom.HorizontalAlignment = Element.ALIGN_LEFT;
                ValcellResCom.VerticalAlignment = Element.ALIGN_MIDDLE;
                ValcellResCom.UseAscender = true;
                ValcellResCom.Padding = 5;
                //ValcellResCom.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;

                PdfPCell ValcellindCom = new PdfPCell(new Phrase(item.indicador, fonEtiqueta_b));
                ValcellindCom.HorizontalAlignment = Element.ALIGN_LEFT;
                ValcellindCom.VerticalAlignment = Element.ALIGN_MIDDLE;
                ValcellindCom.UseAscender = true;
                ValcellindCom.Padding = 5;
                //ValcellindCom.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;

                #region obtner avance mensual
                switch (elMes)
                {
                    case "Enero":
                        valormensual = item.enero_ava;
                        valormensualprogramado = item.enero_mod;
                        break;
                    case "Febrero":
                        valormensual = item.febrero_ava;
                        valormensualprogramado = item.febrero_mod;
                        break;
                    case "Marzo":
                        valormensual = item.marzo_ava;
                        valormensualprogramado = item.marzo_mod;
                        break;
                    case "Abril":
                        valormensual = item.abril_ava;
                        valormensualprogramado = item.abril_mod;
                        break;
                    case "Mayo":
                        valormensual = item.mayo_ava;
                        valormensualprogramado = item.mayo_mod;
                        break;
                    case "Junio":
                        valormensual = item.junio_ava;
                        valormensualprogramado = item.junio_mod;
                        break;
                    case "Julio":
                        valormensual = item.julio_ava;
                        valormensualprogramado = item.julio_mod;
                        break;
                    case "Agosto":
                        valormensual = item.agosto_ava;
                        valormensualprogramado = item.agosto_mod;
                        break;
                    case "Septiembre":
                        valormensual = item.septiembre_ava;
                        valormensualprogramado = item.septiembre_mod;
                        break;
                    case "Octubre":
                        valormensual = item.octubre_ava;
                        valormensualprogramado = item.octubre_mod;
                        break;
                    case "Noviembre":
                        valormensual = item.noviembre_ava;
                        valormensualprogramado = item.noviembre_mod;
                        break;
                    case "Diciembre":
                        valormensual = item.diciembre_ava;
                        valormensualprogramado = item.diciembre_mod;
                        break;
                }
                #endregion

                //PdfPCell ValcellAvaMen = new PdfPCell(new Phrase(valormensual.ToString(), fonEtiqueta_b));
                PdfPCell ValcellProMen = new PdfPCell(new Phrase(valormensualprogramado.ToString(), fonEtiqueta_b));
                ValcellProMen.HorizontalAlignment = Element.ALIGN_CENTER;
                ValcellProMen.VerticalAlignment = Element.ALIGN_MIDDLE;
                ValcellProMen.UseAscender = true;

                PdfPCell ValcellAvaMen = new PdfPCell(new Phrase(valormensual.ToString(), fonEtiqueta_b));
                ValcellAvaMen.HorizontalAlignment = Element.ALIGN_CENTER;
                ValcellAvaMen.VerticalAlignment = Element.ALIGN_MIDDLE;
                ValcellAvaMen.UseAscender = true;
                //cellAvaMen.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;

                PdfPCell ValcellMeta = new PdfPCell(new Phrase(item.totPro.ToString(), fonEtiqueta_b));
                ValcellMeta.HorizontalAlignment = Element.ALIGN_CENTER;
                ValcellMeta.VerticalAlignment = Element.ALIGN_MIDDLE;
                ValcellMeta.UseAscender = true;
                //cellMeta.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;

                //PdfPCell ValcellPorMen = new PdfPCell(new Phrase("% Men", fonEtiqueta_b));
                PdfPCell ValcellPorMen = new PdfPCell(new Phrase(valormensualprogramado == 0 ? "0 %" : ((Convert.ToDouble(valormensual) / Convert.ToDouble(valormensualprogramado)) * 100).ToString("0.##") + " %", fonEtiqueta_b));
                ValcellPorMen.HorizontalAlignment = Element.ALIGN_CENTER;
                ValcellPorMen.VerticalAlignment = Element.ALIGN_MIDDLE;
                ValcellPorMen.UseAscender = true;
                ValcellPorMen.BackgroundColor = new iTextSharp.text.BaseColor(234, 236, 238);
                ValcellPorMen.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);

                PdfPCell ValcellAcu = new PdfPCell(new Phrase(item.totAva.ToString(), fonEtiqueta_b));
                ValcellAcu.HorizontalAlignment = Element.ALIGN_CENTER;
                ValcellAcu.VerticalAlignment = Element.ALIGN_MIDDLE;
                ValcellAcu.UseAscender = true;
                //cellAcu.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;

                //PdfPCell ValcellPor = new PdfPCell(new Phrase(((datosOrganoComponente.totAva / datosOrganoComponente.totPro) * 100).ToString(), fonDato_b));
                PdfPCell ValcellPor = new PdfPCell(new Phrase(((Convert.ToDouble(item.totAva) / Convert.ToDouble(item.totPro)) * 100).ToString("0.##") + " %", fonDato_b));
                ValcellPor.HorizontalAlignment = Element.ALIGN_CENTER;
                ValcellPor.VerticalAlignment = Element.ALIGN_MIDDLE;
                ValcellPor.UseAscender = true;
                ValcellPor.BackgroundColor = new iTextSharp.text.BaseColor(234, 236, 238);
                ValcellPor.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
                //cellPor.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;

                Valintitulocomponente.AddCell(ValcellResCom);
                Valintitulocomponente.AddCell(ValcellindCom);
                Valintitulocomponente.AddCell(ValcellProMen);
                Valintitulocomponente.AddCell(ValcellAvaMen);
                Valintitulocomponente.AddCell(ValcellPorMen);
                Valintitulocomponente.AddCell(ValcellMeta);
                Valintitulocomponente.AddCell(ValcellAcu);
                Valintitulocomponente.AddCell(ValcellPor);

                docPoaMes.Add(Valintitulocomponente);
                #endregion

                #region detallado de las actividades

                #region Titulo Actividad
                PdfPTable tituloactividad = new PdfPTable(1)
                {
                    TotalWidth = 730,
                    LockedWidth = true
                };

                tituloactividad.SetWidths(widthsTitulosGenerales);
                tituloactividad.HorizontalAlignment = 0;
                //titulocomponente.SpacingAfter = 10f;
                titulocomponente.SpacingBefore = 10f;

                PdfPCell cellTituloActividad = new PdfPCell(new Phrase("ACTIVIDAD", fonDato_b));
                cellTituloActividad.HorizontalAlignment = Element.ALIGN_CENTER;
                cellTituloActividad.BackgroundColor = new iTextSharp.text.BaseColor(234, 236, 238);
                cellTituloActividad.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
                cellTituloActividad.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;
                tituloactividad.AddCell(cellTituloActividad);

                docPoaMes.Add(tituloactividad);
                #endregion

                var detalleActividad = repo.Getdosparam1<ImpresionAvanceActividadMensual>("sp_impresion_actividad_indicador_totPro_totAva_avaMensual", new { @idcomponente = item.idComponente }).ToList();

                foreach(var itemAct in detalleActividad)
                {

                    int valormensualActividad = 0;
                    int valorprogramadoactividad = 0;

                    #region ValdentroComponente
                    PdfPTable detAct = new PdfPTable(8)
                    {
                        TotalWidth = 730,
                        LockedWidth = true
                    };

                    float[] valAct = new float[8];
                    valAct[0] = 215;
                    valAct[1] = 215;
                    valAct[2] = 50;
                    valAct[3] = 50;
                    valAct[4] = 50;
                    valAct[5] = 50;
                    valAct[6] = 50;
                    valAct[7] = 50;

                    detAct.SetWidths(valAct);
                    detAct.HorizontalAlignment = 0;
                    //detAct.SpacingAfter = 10f;
                    //intitulocomponente.SpacingBefore = 10f;

                    PdfPCell actObj = new PdfPCell(new Phrase(itemAct.objetivo, fonEtiqueta_b));
                    actObj.HorizontalAlignment = Element.ALIGN_LEFT;
                    actObj.VerticalAlignment = Element.ALIGN_MIDDLE;
                    actObj.UseAscender = true;
                    actObj.Padding = 5;
                    //ValcellResCom.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;

                    PdfPCell actInd = new PdfPCell(new Phrase(itemAct.indicador, fonEtiqueta_b));
                    actInd.HorizontalAlignment = Element.ALIGN_LEFT;
                    actInd.VerticalAlignment = Element.ALIGN_MIDDLE;
                    actInd.UseAscender = true;
                    actInd.Padding = 5;
                    //ValcellindCom.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;

                    #region obtner avance mensual
                    switch (elMes)
                    {
                        case "Enero":
                            valormensualActividad = itemAct.enero_ava;
                            valorprogramadoactividad = itemAct.enero_mod;
                            break;
                        case "Febrero":
                            valormensualActividad = itemAct.febrero_ava;
                            valorprogramadoactividad = itemAct.febrero_mod;
                            break;
                        case "Marzo":
                            valormensualActividad = itemAct.marzo_ava;
                            valorprogramadoactividad = itemAct.marzo_mod;
                            break;
                        case "Abril":
                            valormensualActividad = itemAct.abril_ava;
                            valorprogramadoactividad = itemAct.abril_mod;
                            break;
                        case "Mayo":
                            valormensualActividad = itemAct.mayo_ava;
                            valorprogramadoactividad = itemAct.mayo_mod;
                            break;
                        case "Junio":
                            valormensualActividad = itemAct.junio_ava;
                            valorprogramadoactividad = itemAct.junio_mod;
                            break;
                        case "Julio":
                            valormensualActividad = itemAct.julio_ava;
                            valorprogramadoactividad = itemAct.julio_mod;
                            break;
                        case "Agosto":
                            valormensualActividad = itemAct.agosto_ava;
                            valorprogramadoactividad = itemAct.agosto_mod;
                            break;
                        case "Septiembre":
                            valormensualActividad = itemAct.septiembre_ava;
                            valorprogramadoactividad = itemAct.septiembre_mod;
                            break;
                        case "Octubre":
                            valormensualActividad = itemAct.octubre_ava;
                            valorprogramadoactividad = itemAct.octubre_mod;
                            break;
                        case "Noviembre":
                            valormensualActividad = itemAct.noviembre_ava;
                            valorprogramadoactividad = itemAct.noviembre_mod;
                            break;
                        case "Diciembre":
                            valormensualActividad = itemAct.diciembre_ava;
                            valorprogramadoactividad = itemAct.diciembre_mod;
                            break;
                    }
                    #endregion

                    //PdfPCell actProMen = new PdfPCell(new Phrase(valormensualActividad.ToString(), fonEtiqueta_b));
                    PdfPCell actProMen = new PdfPCell(new Phrase(valorprogramadoactividad.ToString(), fonEtiqueta_b));
                    actProMen.HorizontalAlignment = Element.ALIGN_CENTER;
                    actProMen.VerticalAlignment = Element.ALIGN_MIDDLE;
                    actProMen.UseAscender = true;

                    PdfPCell actAvaMen = new PdfPCell(new Phrase(valormensualActividad.ToString(), fonEtiqueta_b));
                    actAvaMen.HorizontalAlignment = Element.ALIGN_CENTER;
                    actAvaMen.VerticalAlignment = Element.ALIGN_MIDDLE;
                    actAvaMen.UseAscender = true;
                    //cellAvaMen.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;

                    //PdfPCell actPorMen = new PdfPCell(new Phrase(valormensualActividad.ToString(), fonEtiqueta_b));
                    //PdfPCell actPorMen = new PdfPCell(new Phrase("% Men", fonEtiqueta_b));
                    PdfPCell actPorMen = new PdfPCell(new Phrase(valorprogramadoactividad == 0 ? "0 %" : ((Convert.ToDouble(valormensualActividad) / Convert.ToDouble(valorprogramadoactividad)) * 100).ToString("0.##") + " %", fonEtiqueta_b));
                    actPorMen.HorizontalAlignment = Element.ALIGN_CENTER;
                    actPorMen.VerticalAlignment = Element.ALIGN_MIDDLE;
                    actPorMen.UseAscender = true;
                    actPorMen.BackgroundColor = new iTextSharp.text.BaseColor(234, 236, 238);
                    actPorMen.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);

                    PdfPCell actPro = new PdfPCell(new Phrase(itemAct.totPro.ToString(), fonEtiqueta_b));
                    actPro.HorizontalAlignment = Element.ALIGN_CENTER;
                    actPro.VerticalAlignment = Element.ALIGN_MIDDLE;
                    actPro.UseAscender = true;
                    //cellMeta.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;

                    PdfPCell actAva = new PdfPCell(new Phrase(itemAct.totAva.ToString(), fonEtiqueta_b));
                    actAva.HorizontalAlignment = Element.ALIGN_CENTER;
                    actAva.VerticalAlignment = Element.ALIGN_MIDDLE;
                    actAva.UseAscender = true;
                    //cellAcu.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;

                    //PdfPCell actPorAva = new PdfPCell(new Phrase(((Convert.ToDouble(itemAct.totAva) / Convert.ToDouble(itemAct.totPro)) * 100).ToString("0.##"), fonDato_b));
                    PdfPCell actPorAva = new PdfPCell(new Phrase(((Convert.ToDouble(itemAct.totAva) / Convert.ToDouble(itemAct.totPro)) * 100).ToString("F2"), fonDato_b));
                    actPorAva.HorizontalAlignment = Element.ALIGN_CENTER;
                    actPorAva.VerticalAlignment = Element.ALIGN_MIDDLE;
                    actPorAva.UseAscender = true;
                    actPorAva.BackgroundColor = new iTextSharp.text.BaseColor(234, 236, 238);
                    actPorAva.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
                    //cellPor.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;

                    detAct.AddCell(actObj);
                    detAct.AddCell(actInd);
                    detAct.AddCell(actProMen);
                    detAct.AddCell(actAvaMen);
                    detAct.AddCell(actPorMen);
                    detAct.AddCell(actPro);
                    detAct.AddCell(actAva);
                    detAct.AddCell(actPorAva);

                    docPoaMes.Add(detAct);
                    #endregion
                }
                #endregion
            }
            #endregion

            #region Titulo Justificacion
            PdfPTable tituloJustificacion = new PdfPTable(1)
            {
                TotalWidth = 730,
                LockedWidth = true
            };

            tituloJustificacion.SetWidths(widthsTitulosGenerales);
            tituloJustificacion.HorizontalAlignment = 0;
            //titulocomponente.SpacingAfter = 10f;
            tituloJustificacion.SpacingBefore = 10f;

            PdfPCell cellTitJus = new PdfPCell(new Phrase("JUSTIFICACIONES", fonDato));
            cellTitJus.HorizontalAlignment = Element.ALIGN_CENTER;
            cellTitJus.BackgroundColor = new iTextSharp.text.BaseColor(234, 236, 238);
            cellTitJus.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            cellTitJus.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;
            tituloJustificacion.AddCell(cellTitJus);

            docPoaMes.Add(tituloJustificacion);
            #endregion

            #region Ciclo Justificaciones
            var Comp_Just_Area_mes = repo.Getdosparam1<ComponenteJustificacionAreaMes>("sp_justificacion_componente_obtener_por_area_y_mes", new { @idArea = elArea, @mes = elMes }).ToList();

            #region Tabla Titulo Tabla Justificacion 
            foreach (var item in Comp_Just_Area_mes)
            {
                #region titulos tabla justificacion
                PdfPTable tit_jus_com = new PdfPTable(2)
                {
                    TotalWidth = 730,
                    LockedWidth = true
                };

                float[] valuesInComponente = new float[2];
                valuesInComponente[0] = 365;
                valuesInComponente[1] = 365;

                tit_jus_com.SetWidths(valuesInComponente);
                tit_jus_com.HorizontalAlignment = 0;
                //intitulocomponente.SpacingAfter = 10f;
                tit_jus_com.SpacingBefore = 10f;

                PdfPCell cellValCom = new PdfPCell(new Phrase("Resumen Narrativo", fonDato_b));
                cellValCom.HorizontalAlignment = Element.ALIGN_CENTER;
                cellValCom.VerticalAlignment = Element.ALIGN_MIDDLE;
                cellValCom.UseAscender = true;
                cellValCom.BackgroundColor = new iTextSharp.text.BaseColor(234, 236, 238);
                cellValCom.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
                cellValCom.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;

                PdfPCell cellJusCom = new PdfPCell(new Phrase("Justificación", fonDato_b));
                cellJusCom.HorizontalAlignment = Element.ALIGN_CENTER;
                cellJusCom.VerticalAlignment = Element.ALIGN_MIDDLE;
                cellJusCom.UseAscender = true;
                cellJusCom.BackgroundColor = new iTextSharp.text.BaseColor(234, 236, 238);
                cellJusCom.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
                cellJusCom.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;

                tit_jus_com.AddCell(cellValCom);
                tit_jus_com.AddCell(cellJusCom);

                //PdfPTable val_jus_com = new PdfPTable(2)
                //{
                //    TotalWidth = 730,
                //    LockedWidth = true
                //};

                //float[] valuesjus = new float[2];
                //valuesjus[0] = 365;
                //valuesjus[1] = 365;

                //val_jus_com.SetWidths(valuesjus);
                //val_jus_com.HorizontalAlignment = 0;

                PdfPCell ValcellJusCom = new PdfPCell(new Phrase("Comp. " + item.componente, fonEtiqueta_b));
                ValcellJusCom.HorizontalAlignment = Element.ALIGN_LEFT;
                ValcellJusCom.VerticalAlignment = Element.ALIGN_MIDDLE;
                ValcellJusCom.UseAscender = true;
                ValcellJusCom.Padding = 5;

                PdfPCell ValcellJusAna = new PdfPCell(new Phrase(item.analisis, fonEtiqueta_b));
                ValcellJusAna.HorizontalAlignment = Element.ALIGN_LEFT;
                ValcellJusAna.VerticalAlignment = Element.ALIGN_MIDDLE;
                ValcellJusAna.UseAscender = true;
                ValcellJusAna.Padding = 5;

                tit_jus_com.AddCell(ValcellJusCom);
                tit_jus_com.AddCell(ValcellJusAna);

                //intitulocomponente.SpacingAfter = 10f;
                //intitulocomponente.SpacingBefore = 10f;

                docPoaMes.Add(tit_jus_com);

                //---------------------------------------------------------------------------------Justificacion Actividad
                PdfPTable val_jus_act = new PdfPTable(2)
                {
                    TotalWidth = 730,
                    LockedWidth = true
                };

                float[] valuesjusAct = new float[2];
                valuesjusAct[0] = 365;
                valuesjusAct[1] = 365;

                val_jus_act.SetWidths(valuesjusAct);
                val_jus_act.HorizontalAlignment = 0;

                var Comp_Just_Act_Comp_mes = repo.Getdosparam1<ActividadJustificacionComponenteMes>("sp_justificacion_actividad_componente_obtener_por_area_y_mes", new { @idcomponente = item.idComponente, @mes = elMes }).ToList();
                foreach(var itemAct in Comp_Just_Act_Comp_mes)
                {
                    PdfPCell ValCelObj = new PdfPCell(new Phrase("Act: " + itemAct.objetivo, fonEtiqueta_b));
                    ValCelObj.HorizontalAlignment = Element.ALIGN_LEFT;
                    ValCelObj.VerticalAlignment = Element.ALIGN_MIDDLE;
                    ValCelObj.UseAscender = true;
                    ValCelObj.Padding = 5;

                    PdfPCell ValCelAnalisis = new PdfPCell(new Phrase(itemAct.analisis_act, fonEtiqueta_b));
                    ValCelAnalisis.HorizontalAlignment = Element.ALIGN_LEFT;
                    ValCelAnalisis.VerticalAlignment = Element.ALIGN_MIDDLE;
                    ValCelAnalisis.UseAscender = true;
                    ValCelAnalisis.Padding = 5;

                    val_jus_act.AddCell(ValCelObj);
                    val_jus_act.AddCell(ValCelAnalisis);

                    //docPoaMes.Add(val_jus_act);
                }
                docPoaMes.Add(val_jus_act);
                //---------------------------------------------------------------------------------Justificacion Actividad

                #endregion

            }

            #endregion

            #endregion

            docPoaMes.Close();
            byte[] bytesStream = msRep.ToArray();
            msRep = new MemoryStream();
            msRep.Write(bytesStream, 0, bytesStream.Length);
            msRep.Position = 0;

            return new FileStreamResult(msRep, "application/pdf");
        }

        [Authorize(Roles = "Administrador, Enlace")]
        public IActionResult PoaMensualEnlace()
        {
            ViewBag.elAreaEnlace = @SessionHelper.GetOrgano(User);
            ViewBag.losMeses = Constantes.Meses();
            return View();
        }
    }

    public class HeaderFooterPoa : PdfPageEventHelper
    {
        private string _Titulo, _Lider;

        public string titulo
        {
            get { return _Titulo; }
            set { _Titulo = value; }
        }

        public string liderProyecto
        {
            get { return _Lider; }
            set { _Lider = value; }
        }

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            base.OnOpenDocument(writer, document);
        }

        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            Rectangle page = document.PageSize.Rotate();
            string imageizq = @"C:\inetpub\wwwroot\fotoUser\gobedohor.png";
            //var imageizq = "/fotoUsergobedohor.png";
            iTextSharp.text.Image jpgSupIzq = iTextSharp.text.Image.GetInstance(imageizq);
            jpgSupIzq.ScaleToFit(80f, 80f);

            PdfPCell clLogoSupIzq = new PdfPCell();
            clLogoSupIzq.BorderWidth = 0;
            clLogoSupIzq.VerticalAlignment = Element.ALIGN_BOTTOM;
            clLogoSupIzq.AddElement(jpgSupIzq);

            string imageder = @"C:\inetpub\wwwroot\fotoUser\nuevoCeccc.png";
            //var imageder = "/fotoUser/nuevoCeccc.png";
            iTextSharp.text.Image jpgSupDer = iTextSharp.text.Image.GetInstance(imageder);
            jpgSupDer.Alignment = iTextSharp.text.Image.ALIGN_RIGHT;
            jpgSupDer.ScaleToFit(100f, 100f);

            PdfPCell clLogoSupDer = new PdfPCell();
            clLogoSupDer.BorderWidth = 0;
            clLogoSupDer.VerticalAlignment = Element.ALIGN_BOTTOM;
            clLogoSupDer.AddElement(jpgSupDer);

            Chunk chkTit = new Chunk("Unidad de Planeación", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12f, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
            Paragraph paragraph = new Paragraph();
            paragraph.Alignment = Element.ALIGN_CENTER;
            paragraph.Add(chkTit);

            Chunk chkSub = new Chunk(_Titulo, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 11f, iTextSharp.text.Font.BOLD, BaseColor.BLACK));
            Paragraph paragraph1 = new Paragraph();
            paragraph1.Alignment = Element.ALIGN_CENTER;
            paragraph1.Add(chkSub);

            PdfPCell clTitulo = new PdfPCell();
            clTitulo.BorderWidth = 0;
            clTitulo.AddElement(paragraph);

            PdfPCell clSubTit = new PdfPCell();
            clSubTit.BorderWidth = 0;
            clSubTit.AddElement(paragraph1);

            PdfPTable tblTitulo = new PdfPTable(1);
            tblTitulo.WidthPercentage = 100;
            tblTitulo.AddCell(clTitulo);
            tblTitulo.AddCell(clSubTit);

            PdfPCell clTablaTitulo = new PdfPCell();
            clTablaTitulo.BorderWidth = 0;
            clTablaTitulo.VerticalAlignment = Element.ALIGN_MIDDLE;
            clTablaTitulo.AddElement(tblTitulo);

            PdfPTable tblEncabezado = new PdfPTable(3);
            tblEncabezado.WidthPercentage = 100;
            float[] widths = new float[] { 20f, 60f, 20f };
            tblEncabezado.SetWidths(widths);

            tblEncabezado.AddCell(clLogoSupIzq);
            tblEncabezado.AddCell(clTablaTitulo);
            tblEncabezado.AddCell(clLogoSupDer);

            base.OnOpenDocument(writer, document);

            PdfPTable tabFot = new PdfPTable(new float[] { 1F });
            tabFot.SpacingAfter = 5F;
            PdfPCell cell;
            //ancho de la tabla
            tabFot.TotalWidth = 730;
            cell = new PdfPCell(tblEncabezado);
            cell.Border = Rectangle.NO_BORDER;
            tabFot.AddCell(cell);
            //tabFot.WriteSelectedRows(0, -1, 20, document.Top + tabFot.TotalHeight + 10, writer.DirectContent);
            tabFot.WriteSelectedRows(0, -1, 20, document.Top + tabFot.TotalHeight + 10, writer.DirectContent);
            tabFot.SpacingAfter = 30f;

            var fontFooter = FontFactory.GetFont("Verdana", 8, Font.NORMAL, BaseColor.BLACK);
            var fontFooterTitulo = FontFactory.GetFont("Verdana", 8, Font.BOLD, BaseColor.BLACK);

            PdfPTable footer = new PdfPTable(3);
            //footer.TotalWidth = page.Width + 40;
            footer.TotalWidth = 730;

            //PdfPCell cf1 = new PdfPCell(new Phrase("Folio", fontFooter));
            PdfPCell cf1 = new PdfPCell(new Phrase("", fontFooter));
            cf1.HorizontalAlignment = Element.ALIGN_CENTER;
            cf1.Border = PdfPCell.NO_BORDER;
            cf1.BorderWidthTop = 0.75f;
            footer.AddCell(cf1);

            PdfPCell cf2 = new PdfPCell(new Phrase(_Lider, fontFooter));
            cf2.HorizontalAlignment = Element.ALIGN_CENTER;
            cf2.Border = PdfPCell.NO_BORDER;
            cf2.BorderWidthTop = 0.75f;
            footer.AddCell(cf2);

            PdfPCell cf3 = new PdfPCell(new Phrase(DateTime.Now.ToShortDateString(), fontFooter));
            cf3.HorizontalAlignment = Element.ALIGN_CENTER;
            cf3.Border = PdfPCell.NO_BORDER;
            cf3.BorderWidthTop = 0.75f;
            footer.AddCell(cf3);

            //PdfPCell cf1b = new PdfPCell(new Phrase("El folio", fontFooter));
            PdfPCell cf1b = new PdfPCell(new Phrase("", fontFooter));
            cf1b.HorizontalAlignment = Element.ALIGN_CENTER;
            cf1b.Border = PdfPCell.NO_BORDER;
            footer.AddCell(cf1b);

            PdfPCell cf2b = new PdfPCell(new Phrase("Lidel del proyecto", fontFooter));
            cf2b.HorizontalAlignment = Element.ALIGN_CENTER;
            cf2b.Border = PdfPCell.NO_BORDER;
            footer.AddCell(cf2b);

            PdfPCell cf3b = new PdfPCell(new Phrase("Fecha impresión", fontFooter));
            cf3b.HorizontalAlignment = Element.ALIGN_CENTER;
            cf3b.Border = PdfPCell.NO_BORDER;
            footer.AddCell(cf3b);

            //PdfPCell texto = new PdfPCell(new Phrase("Este documento es confidencial no tendrá ningún valor jurídico si presenta tachaduras o enmendaduras.", fontFooter));
            //texto.Colspan = 3;
            //texto.Border = PdfPCell.NO_BORDER;
            //texto.HorizontalAlignment = Element.ALIGN_CENTER;
            //footer.AddCell(texto);

            footer.WriteSelectedRows(0, -1, 20, 60, writer.DirectContent);
            //                                  60 margen inferior

            iTextSharp.text.Rectangle rect = writer.GetBoxSize("footer");
        }

        public static HeaderFooterPoa getMultilineFooter(string Titulo, string Lider)
        {
            HeaderFooterPoa result = new HeaderFooterPoa();

            result.titulo = Titulo;
            result.liderProyecto = Lider;

            return result;
        }
    }
}
