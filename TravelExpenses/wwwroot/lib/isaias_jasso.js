var arr = [];
function ReiniciarPagina() {
    window.location = "../Comprobacion/SolicitudCerradas";

}
function ExportalExcel() {

    var url = "/Comprobacion/OnPostExport/";
    var parameto = arr;

    var parameters = {
        parameto
    };

    if (arr.length > 0) {

        $.ajax({
            url: url,
            data: parameters,
            type: "POST",
            cache: false,
            success: function (response) {
                debugger;
                alert("Excel Descargado Exitosamente");
                document.location = response;
            },
            error: function (error) {
                alert(error);
            }
        });
    }
    
    }

function SelecionarSolicitudes(Folio,c) {
    debugger;
    var url = "/Comprobacion/SelecionarSolicitudes/";
    var parameto = document.getElementById('Array').value;
    var valor = document.getElementById('SOLICITIDCHECK['+c+']').checked;
    $.ajax({
        url: url,
        data: { parameto: parameto, Folio: Folio, valor: valor},
        type: "POST",
        cache: false,
        success: function (response) {
            document.getElementById('Array').value = JSON.stringify(response);
        },
        error: function (error) {
            alert(error);
        }
    });
 
}


function ObtenerSolcitudesFechas() {
    var estatus = $("#estatus").children("option:selected").text();
    var inicio = document.getElementById('inicio').value;
    var fin = document.getElementById('fin').value;
    var row = "";
    var url = "/Comprobacion/SolicitudesEstatusFechas/";
    $.ajax({
        url: url,
        data: { estatus: estatus, inicio: inicio, fin: fin },
        type: "POST",
        cache: false,
        success: function (response) {
            var data = [];
            data = response;
            arr = response;
            document.getElementById('Array').value = JSON.stringify(response);
            var row = "";
            if (data.length == 0) {
                row += "<tr><td colspan='9'>" + "-- No existen registros --" + "</td></tr>";
            }
            else {
                var contador = 0;
                $.each(response, function (index, item) {
                    if (item.estatus == "PorLiberar") {
                        row +=
                            '<tr>'
                        + '<td><input type="checkbox" id="SOLICITIDCHECK[' + contador + ']" name="SOLICITIDCHECK" onclick="SelecionarSolicitudes(' + item.folio + ',' + contador +')"/></td>'
                            + '<td>' + item.folio + '</td>'
                            + '<td>' + item.descripcion + '</td>'
                            + '<td>' + item.userName + '</td>'
                            + '<td>' + item.importeSolicitado + '</td>'
                            + '<td>' + item.importeComprobadoC + '</td>'
                            + '<td>' + item.estatus + '</td>'
                            + '<td>'
                            + '<a class="btn btn-lg" href="/Comprobacion/DetallesPorAutorizar?folio=' + item.folio + '">'
                            + "<i class='fas fa-edit'></i>"
                            + '</a>'
                            + '</td>'
                            + '</tr>';
                    }
                    else if (item.estatus == "Revisada") {

                        row +=
                            '<tr>'
                        + '<td><input type="checkbox" id="SOLICITIDCHECK[' + contador + ']" name="SOLICITIDCHECK" onclick="SelecionarSolicitudes(' + item.folio + ',' + contador + ')"/></td>'

                            + '<td>' + item.folio + '</td>'
                            + '<td>' + item.descripcion + '</td>'
                            + '<td>' + item.userName + '</td>'
                            + '<td>' + item.importeSolicitadoC + '</td>'
                            + '<td>' + item.importeComprobadoC + '</td>'
                            + '<td>' + item.estatus + '</td>'
                            + '<td>'
                            + '<a class="btn btn-lg"  href="/Comprobacion/Detalles?folio=' + item.folio + '">'
                            + "<i class='fas fa-edit'></i>"
                            + '</a>'
                            + '</td>'
                            + '</tr>';
                    }
                    else if (item.estatus == "PorComprobar") {

                        row +=
                            '<tr>'
                        + '<td><input type="checkbox" id="SOLICITIDCHECK[' + contador + ']" name="SOLICITIDCHECK" onclick="SelecionarSolicitudes(' + item.folio + ',' + contador + ')"/></td>'

                            + '<td>' + item.folio + '</td>'
                            + '<td>' + item.descripcion + '</td>'
                            + '<td>' + item.userName + '</td>'
                            + '<td>' + item.importeSolicitadoC + '</td>'
                            + '<td>' + item.importeComprobadoC + '</td>'
                            + '<td>' + item.estatus + '</td>'
                            + '<td>'
                            + '<a class="btn btn-lg"  href="/Comprobacion/Detalles?folio=' + item.folio + '">'
                            + "<i class='fas fa-edit'></i>"
                            + '</a>'
                            + '</td>'
                            + '</tr>';
                    }
                    else if (item.estatus == "Cerrada") {
                        row +=
                            '<tr>'
                        + '<td><input type="checkbox" id="SOLICITIDCHECK[' + contador + ']" name="SOLICITIDCHECK" onclick="SelecionarSolicitudes(' + item.folio + ',' + contador + ')"/></td>'

                            + '<td>' + item.folio + '</td>'
                            + '<td>' + item.descripcion + '</td>'
                            + '<td>' + item.userName + '</td>'
                            + '<td>' + item.importeSolicitado + '</td>'
                            + '<td>' + item.FechaSolicitud + '</td>'
                            + '<td>' + item.importeComprobadoC + '</td>'
                            + '<td>' + item.estatus + '</td>'
                            + '<td>'
                            + '<a class="btn btn-lg"  href="/Comprobacion/DetallesSolicitudCerradas?folio=' + item.folio + '">'
                            + "<i class='fas fa-edit'></i>"
                            + '</a>'
                            + '</td>'
                            + '</tr>';
                    }
                    document.getElementById('SOLICITIDCHECK[' + contador + ']').checked = item.ExportarRealizada;
                    document.getElementById('SOLICITIDCHECK[' + contador + ']').disabled = item.ExportarRealizada;
                    contador++;
                });
            }
            $("#tblSolicitud").html(row);
            var contador1 = 0;
            $.each(response, function (index, item) {
                if (item.exportarRealizada == true) {
                    document.getElementById('SOLICITIDCHECK[' + contador1 + ']').checked = item.exportarRealizada;
                    document.getElementById('SOLICITIDCHECK[' + contador1 + ']').disabled = item.exportarRealizada;
                }
                contador1++;
            });
        },
        error: function (error) {
            alert("unknown error: ", error);
        }
    });
}
function ObtenerSolcitudes() {
    var estatus = $("#estatus").children("option:selected").text();
    var row = "";
    var url = "/Comprobacion/SolicitudesEstatus/";
    $.ajax({
        url: url,
        data: { estatus: estatus },
        type: "POST",
        cache: false,
        success: function (response) {
            var data = [];
            data = response;
            arr = response;
            document.getElementById('Array').value = JSON.stringify(response);
            var row = "";
            if (data.length == 0) {
                row += "<tr><td colspan='9'>" + "-- No existen registros --" + "</td></tr>";
            }
            else {
                
                var contador = 0;
                $.each(response, function (index, item) {
                    if (item.estatus == "PorLiberar") {
                        row +=
                            '<tr>'
                        + '<td><input type="checkbox" id="SOLICITIDCHECK[' + contador + ']" name="SOLICITIDCHECK" onclick="SelecionarSolicitudes(' + item.folio + ',' + contador + ')"/></td>'
                            + '<td>' + item.folio + '</td>'
                            + '<td>' + item.descripcion + '</td>'
                            + '<td>' + item.userName + '</td>'
                            + '<td>' + item.importeSolicitadoC + '</td>'
                            + '<td>' + item.importeComprobadoC + '</td>'
                            + '<td>' + item.estatus + '</td>'
                            + '<td>'
                            + '<a class="btn btn-lg" href="/Comprobacion/DetallesPorAutorizar?folio=' + item.folio + '">'
                            + "<i class='fas fa-edit'></i>"
                            + '</a>'
                            + '</td>'
                            + '</tr>';
                    }
                    else if (item.estatus == "Revisada") {

                        row +=
                            '<tr>'
                        + '<td><input type="checkbox" id="SOLICITIDCHECK[' + contador + ']" name="SOLICITIDCHECK" onclick="SelecionarSolicitudes(' + item.folio + ',' + contador + ')"/></td>'
                            + '<td>' + item.folio + '</td>'
                            + '<td>' + item.descripcion + '</td>'
                            + '<td>' + item.userName + '</td>'
                            + '<td>' + item.importeSolicitadoC + '</td>'
                            + '<td>' + item.importeComprobadoC + '</td>'
                            + '<td>' + item.estatus + '</td>'
                            + '<td>'
                            + '<a class="btn btn-lg"  href="/Comprobacion/Detalles?folio=' + item.folio + '">'
                            + "<i class='fas fa-edit'></i>"
                            + '</a>'
                            + '</td>'
                            + '</tr>';
                    } else if (item.estatus == "PorComprobar") {

                        row +=
                            '<tr>'
                        + '<td><input type="checkbox" id="SOLICITIDCHECK[' + contador + ']" name="SOLICITIDCHECK" onclick="SelecionarSolicitudes(' + item.folio + ',' + contador + ')"/></td>'
                            + '<td>' + item.folio + '</td>'
                            + '<td>' + item.descripcion + '</td>'
                            + '<td>' + item.userName + '</td>'
                            + '<td>' + item.importeSolicitado + '</td>'
                            + '<td>' + item.importeComprobadoC + '</td>'
                            + '<td>' + item.estatus + '</td>'
                            + '<td>'
                            + '<a class="btn btn-lg"  href="/Comprobacion/Detalles?folio=' + item.folio + '">'
                            + "<i class='fas fa-edit'></i>"
                            + '</a>'
                            + '</td>'
                            + '</tr>';
                    }
                    else if (item.estatus == "Cerrada") {
                        row +=
                            '<tr>'
                        + '<td><input type="checkbox" id="SOLICITIDCHECK[' + contador + ']" name="SOLICITIDCHECK" onclick="SelecionarSolicitudes(' + item.folio + ',' + contador + ')"/></td>'
                            + '<td>' + item.folio + '</td>'
                            + '<td>' + item.descripcion + '</td>'
                            + '<td>' + item.userName + '</td>'
                            + '<td>' + item.importeSolicitadoC + '</td>'
                            + '<td>' + item.importeComprobadoC + '</td>'
                            + '<td>' + item.estatus + '</td>'
                            + '<td>'
                            + '<a class="btn btn-lg"  href="/Comprobacion/DetallesSolicitudCerradas?folio=' + item.folio + '">'
                            + "<i class='fas fa-edit'></i>"
                            + '</a>'
                            + '</td>'
                            + '</tr>';
                    } 
                    
                    
                    contador++;

                });

            }
            $("#tblSolicitud").html(row);
            var contador1 = 0;
            $.each(response, function (index, item) {
                if (item.exportarRealizada == true) {
                    document.getElementById('SOLICITIDCHECK[' + contador1 + ']').checked = item.exportarRealizada;
                    document.getElementById('SOLICITIDCHECK[' + contador1 + ']').disabled = item.exportarRealizada;
                }
                contador1++;
            });
            

        },
        error: function (error) {
            alert("unknown error: ", error);
        }
    });
}

// Basic example
$(document).ready(function () {
    $('#dtBasicExample').DataTable({
        "pagingType": "simple", // "simple" option for 'Previous' and 'Next' buttons only

    });
    $('.dataTables_length').addClass('bs-select');
});


function fnExcelReport() {
    var tab_text = "<table border='1px'><tr>";
    var textRange; var j = 0;
    tab = document.getElementById('tabla2'); // id of table
    if (tab.rows.length > 0) {
        for (j = 0; j < tab.rows.length; j++) {
            tab_text = tab_text + "<th></th>";
            tab_text = tab_text + "<th>" + tab.rows[j].cells[0].innerHTML + "</th>";
            tab_text = tab_text + "<th >" + tab.rows[j].cells[1].innerHTML + "</th>";
            tab_text = tab_text + "<th >" + tab.rows[j].cells[2].innerHTML + "</th>";
            tab_text = tab_text + "<th >" + tab.rows[j].cells[3].innerHTML + "</th>";
            tab_text = tab_text + "<th >" + tab.rows[j].cells[4].innerHTML + "</th></tr>";
        }
    } else {
        alert("Error al Exportar el excel ");
    }
    tab_text = tab_text + "</table>";
    tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");//remove if u want links in your table
    tab_text = tab_text.replace(/<img[^>]*>/gi, ""); // remove if u want images in your table
    tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, ""); // reomves input params

    var ua = window.navigator.userAgent;
    var msie = ua.indexOf("MSIE ");
    debugger;
    if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))      // If Internet Explorer
    {
        txtArea1.document.open("txt/html", "replace");
        txtArea1.document.write(tab_text);
        txtArea1.document.close();
        txtArea1.focus();
        sa = txtArea1.document.execCommand("SaveAs", true, "download.xls");
    }
    else                 //other browser not tested on IE 11
        sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(tab_text));


    return (sa);
}