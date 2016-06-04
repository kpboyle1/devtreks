<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2013, January -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:y0="urn:DevTreks-support-schemas:Input"
	xmlns:DisplayDevPacks="urn:displaydevpacks">
	<xsl:output method="xml" indent="yes" omit-xml-declaration="yes" encoding="UTF-8" />
	<!-- pass in params -->
	<!-- what action is being taken by the server -->
	<xsl:param name="serverActionType" />
	<!-- what other action is being taken by the server -->
	<xsl:param name="serverSubActionType" />
	<!-- is the member viewing this uri the owner? -->
	<xsl:param name="isURIOwningClub" />
	<!-- which node to start with?  (in the case of customdocs this will be a DEVDOCS_TYPES) -->
	<xsl:param name="nodeName" />
	<!-- which view to use? -->
	<xsl:param name="viewEditType" />
	<!-- is this a coordinator? -->
	<xsl:param name="memberRole" />
	<!-- what is the current uri? -->
	<xsl:param name="selectedFileURIPattern" />
	<!-- the addin being used -->
	<xsl:param name="calcDocURI" />
	<!-- the node being calculated (in the case of customdocs this will be a DEVDOCS_TYPES) -->
	<xsl:param name="docToCalcNodeName" />
	<!-- standard params used with calcs and custom docs -->
	<xsl:param name="calcParams" />
	<!-- what is the name of the node to be selected? -->
	<xsl:param name="selectionsNodeNeededName" />
	<!-- which network is this doc from? -->
	<xsl:param name="networkId" />
	<!-- what is the start row? -->
	<xsl:param name="startRow" />
	<!-- what is the end row? -->
	<xsl:param name="endRow" />
	<!-- what are the pagination properties ? -->
	<xsl:param name="pageParams" />
	<!-- what is the guide's email? -->
	<xsl:param name="clubEmail" />
	<!-- what is the current serviceid? -->
	<xsl:param name="contenturipattern" />
	<!-- path to resources -->
	<xsl:param name="fullFilePath" />
	<!-- init html -->
	<xsl:template match="@*|/|node()" />
	<xsl:template match="/">
		<xsl:apply-templates select="root" />
	</xsl:template>
	<xsl:template match="root">
		<div id="mainwrapper">
			<table class="data" cellpadding="6" cellspacing="1" border="0">
				<tbody>
					<xsl:apply-templates select="servicebase" />
					<xsl:apply-templates select="inputgroup" />
					<tr id="footer">
						<td scope="row" colspan="9">
							<a id="aFeedback" name="Feedback">
								<xsl:attribute name="href">mailto:<xsl:value-of select="$clubEmail" />?subject=<xsl:value-of select="$selectedFileURIPattern" /></xsl:attribute>
								Feedback About <xsl:value-of select="$selectedFileURIPattern" />
							</a>
						</td>
					</tr>
				</tbody>
			</table>
		</div>
	</xsl:template>
	<xsl:template match="servicebase">
		<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPacks:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
		<tr>
			<th colspan="9">
				Service: <xsl:value-of select="@Name" />
			</th>
		</tr>
		<xsl:apply-templates select="inputgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="inputgroup">
		<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPacks:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
		<tr>
			<th scope="col" colspan="8">
				Input Group : <xsl:value-of select="@Name" />
			</th>
		</tr>
		<tr>
			<td scope="row" colspan="8">
					Document Status : <xsl:value-of select="DisplayDevPacks:WriteSelectListForStatus($searchurl, @DocStatus, $viewEditType)"/>
			</td>
		</tr>
		<tr>
			<td colspan="8">
					Description : <xsl:value-of select="@Description" />
			</td>
		</tr>
		<tr>
      <td scope="row" colspan="4">
        Label : <xsl:value-of select="@Num" />
      </td>
			<td scope="row" colspan="4">
				Input Type : <xsl:value-of select="DisplayDevPacks:WriteSelectListForTypes($searchurl, @ServiceId, @TypeId, 'TypeId', $viewEditType)"/>
			</td>
		</tr>
		<xsl:apply-templates select="input">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="input">
		<tr>
			<th scope="col" colspan="9">Input</th>
		</tr>
		<tr>
			<th scope="col">Date Applied</th>
			<th scope="col">Label</th>
			<th scope="col">OC Amount</th>
			<th scope="col">OC Unit</th>
			<th scope="col">OC Price</th>
			<th scope="col">AOH Unit</th>
			<th scope="col">AOH Price</th>
			<th scope="col">CAP Unit</th>
			<th scope="col">CAP Price</th>
		</tr>
		<tr>
			<td colspan="9">
				<strong><xsl:value-of select="@Name" /></strong> (<xsl:value-of select="@LastChangedDate" />)
			</td>
		</tr>
		<tr>
			<td>
				<xsl:value-of select="@InputDate" />
			</td>
			<td>
				<xsl:value-of select="@Num" />
			</td>
			<td>
        <xsl:value-of select="@InputPrice1Amount" />
			</td>
			<td>
				<xsl:value-of select="@InputUnit1" />
			</td>
			<td>
				<xsl:value-of select="@InputPrice1" />
			</td>
			<td>
				<xsl:value-of select="@InputUnit2" />
			</td>
			<td>
				<xsl:value-of select="@InputPrice2" />
			</td>
			<td>
				<xsl:value-of select="@InputUnit3" />
			</td>
			<td>
				<xsl:value-of select="@InputPrice3" />
			</td>
		</tr>
			<tr>
				<td colspan="9">
					<strong>
						<label for="lblDescription" >Description</label>
					</strong>
					<br />
					<xsl:value-of select="@Description" />
				</td>
			</tr>
			<tr>
				<th scope="col" colspan="9">Input Series</th>
			</tr>
			<xsl:apply-templates select="inputseries">
				<xsl:sort select="@InputDate"/>
			</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="inputseries">
		<tr>
			<td colspan="9"> 
				<strong><xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<tr>
			<td>
				<xsl:value-of select="@InputDate" />
			</td>
			<td>
				<xsl:value-of select="@Num" />
			</td>
			<td>
        <xsl:value-of select="@InputPrice1Amount" />
			</td>
			<td>
				<xsl:value-of select="@InputUnit1" />
			</td>
			<td>
				<xsl:value-of select="@InputPrice1" />
			</td>
			<td>
				<xsl:value-of select="@InputUnit2" />
			</td>
			<td>
				<xsl:value-of select="@InputPrice2" />
			</td>
			<td>
				<xsl:value-of select="@InputUnit3" />
			</td>
			<td>
				<xsl:value-of select="@InputPrice3" />
			</td>
		</tr>
		<tr>
			<td colspan="1">
				<strong>
					<label for="lblDescription" >Description</label>
				</strong>
			</td>
			<td colspan="7">
				<xsl:value-of select="@Description" />
			</td>
		</tr>
	</xsl:template>
</xsl:stylesheet>