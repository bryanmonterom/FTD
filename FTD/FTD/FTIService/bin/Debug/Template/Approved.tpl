<html >
<head>
	<meta http-equiv="Content-Language" content="en-us">
	<meta name="GENERATOR" content="Microsoft FrontPage 5.0">
	<meta name="ProgId" content="FrontPage.Editor.Document">
	<meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
    <style>
		body{
			font-family: "Helvetica Neue",Helvetica,Arial,sans-serif;
			font-size: 14px;
			line-height: 20px;
		}
		table {
  max-width: 100%;
  background-color: transparent;
  border-collapse: collapse;
  border-spacing: 0;
}

.table {
  width: 500px;
  margin-bottom: 20px;
}

.table th,
.table td {
  padding: 8px;
  line-height: 20px;
  text-align: left;
  vertical-align: top;
  border-top: 1px solid #dddddd;
}

.table th {
  font-weight: bold;
  background-color:#0063BE;
  font: bold 12px;
  color: #ffffff
  }

.table thead th {
  vertical-align: bottom;
}

.table caption + thead tr:first-child th,
.table caption + thead tr:first-child td,
.table colgroup + thead tr:first-child th,
.table colgroup + thead tr:first-child td,
.table thead:first-child tr:first-child th,
.table thead:first-child tr:first-child td {
  border-top: 0;
}

.table tbody + tbody {
  border-top: 2px solid #dddddd;
}

.table .table {
  background-color: #ffffff;
}

.Alert{
 color:red;
 font-size:20px;
 font-style: italic;
 font-family: Verdana, Geneva, Tahoma, sans-serif
}

	</style>
</head>

<body>
<div>
<h1>Universidad Nacional Pedro Henriquez Urena</h1>
</div>
<h2>Tu solicitud ha sido [%Status]</h2>
<br/>
<h4>[%Approver]</h2>
<br/>
<p>
Detalles de la Solicitud
</p>
<table class="table">
  <thead>
    <tr>
        <th>
        Identificador
      </th>
	  <th>
	    Solicitante
    </th>
	  <th>
	    Tipo
	  </th>
	  <th>
        Fecha
      </th>
      <th>
        Paso Aprobado
       </th>
	</tr>
  </thead>
  <tbody>
    [%TRS]
  </tbody>
</table>


<p class="Alert">
No responda este correo.
</p>
</html>



