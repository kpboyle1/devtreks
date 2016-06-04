<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2013, December -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Outcome"
	xmlns:DisplayDevPacks="urn:displaydevpacks">
	<xsl:output method="xml" indent="yes" omit-xml-declaration="yes" encoding="UTF-8" />
	<!-- pass in params -->
	<!-- what action is being taken by the server -->
	<xsl:param name="serverActionType" />
	<!-- what other action is being taken by the server -->
	<xsl:param name="serverSubActionType" />
	<!-- is the member viewing this uri the owner? -->
	<xsl:param name="isURIOwningClub" />
	<!-- which node to start with? -->
	<xsl:param name="nodeName" />
	<!-- which view to use? -->
	<xsl:param name="viewEditType" />
	<!-- is this a coordinator? -->
	<xsl:param name="memberRole" />
	<!-- what is the current uri? -->
	<xsl:param name="selectedFileURIPattern" />
	<!-- the addin being used -->
	<xsl:param name="calcDocURI" />
	<!-- the node being calculated-->
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
					<xsl:apply-templates select="outcomegroup" />
					<tr id="footer">
						<td scope="row" colspan="8">
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
			<th colspan="8">
				Service: <xsl:value-of select="@Name" />
			</th>
		</tr>
		<xsl:apply-templates select="outcomegroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outcomegroup">
		  <xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPacks:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
			<tr>
				<th scope="col" colspan="8">
					Outcome Group : <xsl:value-of select="@Name" />
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
					Outcome Type : <xsl:value-of select="DisplayDevPacks:WriteSelectListForTypes($searchurl, @ServiceId, @TypeId, 'TypeId', $viewEditType)"/>
				</td>
			</tr>
			<xsl:apply-templates select="outcome">
				<xsl:sort select="@Date"/>
			</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outcome">
		<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPacks:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
		<tr>
			<th scope="col" colspan="8">Outcome</th>
		</tr>
		<tr>
			<th scope="col">
				Date&#xA0;Applied
			</th>
			<th scope="col">
				Label&#xA0;1
			</th>
			<th scope="col">
				Label&#xA0;2
			</th>
			<th scope="col">
				Amount
			</th>
			<th scope="col">
				Eff. Life
			</th>
			<th scope="col">
				Salv. Value
			</th>
			<th scope="col">
				Incent. Amount
			</th>
			<th scope="col">
				Incent. Rate
			</th>
		</tr>
		<tr>
			<td scope="row" colspan="8">
				<strong>
          <xsl:value-of select="@Name" /> (<xsl:value-of select="@LastChangedDate" />)
				</strong>
			</td>
		</tr>
		<tr>
			<td>
				<xsl:value-of select="@Date" />
			</td>
			<td>
				<xsl:value-of select="@Num" />
			</td>
			<td>
				<xsl:value-of select="@Num2" />
			</td>
			<td>
        <xsl:value-of select="@Amount" />
			</td>
			<td>
				<xsl:value-of select="@EffectiveLife" />
			</td>
			<td>
				<xsl:value-of select="@SalvageValue" />
			</td>
			<td>
        <xsl:value-of select="@IncentiveAmount" />
			</td>
			<td>
        <xsl:value-of select="@IncentiveRate" />
			</td>
		</tr>
			<tr>
				<td scope="row" colspan="3">
					Operation Unit:<xsl:value-of select="@Unit"/>
				</td>
				<td colspan="1">
					ResourceWeight
				</td>
				<td>
					<xsl:value-of select="@ResourceWeight" />
				</td>
				<td colspan="2">
					Rates (R and N)
				</td>
				<td colspan="1">
					<xsl:value-of select="@RealRate" />&#xA0;&#xA0;<xsl:value-of select="@NominalRate" />
				</td>
			</tr>
			<tr>
				<td colspan="1">
					Description
				</td>
				<td colspan="7">
					<xsl:value-of select="@Description" />
				</td>
			</tr>
			<tr>
				<th scope="col" colspan="2"><strong>Total Benefits - Outcome </strong></th>
				<th scope="col" colspan="2"><strong>Total Benefit </strong></th>
				<th scope="col" colspan="2"><strong>Annual Benefit </strong></th>
				<th scope="col" colspan="2"><strong>Interest Portion </strong></th>
			</tr>
			<tr>
				<td scope="row" colspan="2"><strong>Total Benefits </strong></td>
				<td align="right" colspan="2"><xsl:value-of select="@TR"/></td>
				<td align="right" colspan="2"><xsl:value-of select="@TAMR"/></td>
				<td align="right" colspan="2"><xsl:value-of select="@TR_INT"/></td>
			</tr>
			<tr>
				<td scope="row" colspan="2"><strong>Total Benefits - Outcome w. Incentives </strong> </td>
				<td align="right" colspan="2"><xsl:value-of select="@TRINCENT"/></td>
				<td align="right" colspan="2"><xsl:value-of select="@TAMRINCENT"/></td>
				<td align="right" colspan="2"><xsl:value-of select="@TAMR_INT"/></td>
			</tr>
			<tr>
				<th scope="col" colspan="8">Outputs</th>
			</tr>
		<xsl:apply-templates select="outcomeoutput">
			<xsl:sort select="@OutputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outcomeoutput">
		<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPacks:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
		<tr>
			<th scope="col">
				Output Name
			</th>
			<th scope="col">
				Date&#xA0;Applied
			</th>
			<th scope="col">
				Times
			</th>
			<th scope="col">
				Incent. Amount
			</th>
			<th scope="col">
				Incent. Rate
			</th>
			<th scope="col"></th>
			<th scope="col"></th>
			<th scope="col"></th>
		</tr>
		<tr>
			<td scope="row" colspan="8">
				<strong><xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<tr>
			<td scope="row" colspan="1"></td>
			<td>
				<xsl:value-of select="@OutputDate" />
			</td>
			<td>
				<xsl:value-of select="@OutputTimes" />
			</td>
			<td>
        <xsl:value-of select="@IncentiveAmount" />
			</td>
			<td>
        <xsl:value-of select="@IncentiveRate" />
			</td>
			<td></td>
			<td></td>
			<td></td>
		</tr>
			<tr>
				<th scope="col"><strong>Compos. Amount</strong></th>
				<th scope="col"><strong>Compos. Unit</strong></th>
				<th scope="col"><strong>Amount</strong></th>
				<th scope="col"><strong>Unit</strong></th>
				<th scope="col"><strong>Price</strong></th>
				<th scope="col"><strong>Total </strong></th>
				<th scope="col"><strong>Interest </strong></th>
				<th scope="col"><strong>Total Benefit </strong></th>
			</tr>
			<tr>
				<td>
            <xsl:value-of select="@OutputCompositionAmount" />
				</td>
        <td>
					<xsl:value-of select="@OutputCompositionUnit" />
				</td>
				<td>
          <xsl:value-of select="@OutputAmount1" />
				</td>
				<td>
          <xsl:value-of select="@OutputUnit1" />
				</td>
				<td>
					<xsl:value-of select="@OutputPrice1" />
				</td>
				<td align="right"><xsl:value-of select="@TR"/></td>
				<td align="right"><xsl:value-of select="@TR_INT"/></td>
				<td align="right"><xsl:value-of select="DisplayDevPacks:GetAddNumberN2(@TR, @TR_INT, 0 , 0)"/></td>
			</tr>
			<tr>
				<td colspan="2"><strong>Total Benefits with Incentives </strong></td>
				<td></td>
				<td></td>
				<td></td>
				<td></td>
				<td></td>
				<td align="right"><xsl:value-of select="@TRINCENT"/></td>
			</tr>
			<tr>
				<td colspan="1">
					<strong>
						Description
					</strong>
				</td>
				<td colspan="7">
					<xsl:value-of select="@Description" />
				</td>
			</tr>
	</xsl:template>
</xsl:stylesheet>
