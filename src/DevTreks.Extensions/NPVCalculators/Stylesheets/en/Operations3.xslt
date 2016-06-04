<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2013, December -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Operation"
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
	<!-- is this a cost list or an actual operation? 
	<xsl:variable name="isPriceList" select="//operationgroup/@PriceListYorN"/>-->
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
					<xsl:apply-templates select="operationgroup" />
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
		<xsl:apply-templates select="operationgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="operationgroup">
		  <xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPacks:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
			<tr>
				<th scope="col" colspan="8">
					Operation Group : <xsl:value-of select="@Name" />
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
					Operation Type : <xsl:value-of select="DisplayDevPacks:WriteSelectListForTypes($searchurl, @ServiceId, @TypeId, 'TypeId', $viewEditType)"/>
				</td>
			</tr>
			<tr>
				<td scope="row" colspan="8">
					Is Price List? : <xsl:value-of select="@PriceListYorN" />
				</td>
			</tr>
			<xsl:apply-templates select="operation">
				<xsl:sort select="@Date"/>
			</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="operation">
			<tr>
				<th scope="col" colspan="8">Operation</th>
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
					<th scope="col" colspan="2"><strong>Total Costs - Operation </strong></th>
					<th scope="col" colspan="2"><strong>Total Cost </strong></th>
					<th scope="col" colspan="2"><strong>Annual Cost </strong></th>
					<th scope="col" colspan="2"><strong>Interest Portion </strong></th>
				</tr>
				<tr>
					<td scope="row" colspan="2"><strong>Total Operating Costs </strong></td>
					<td align="right" colspan="2"><xsl:value-of select="@TOC"/></td>
					<td align="right" colspan="2"><xsl:value-of select="@TAMOC"/></td>
					<td align="right" colspan="2"><xsl:value-of select="@TOC_INT"/></td>
				</tr>
				<tr>
					<td scope="row" colspan="2"><strong>Total Allocated Overhead Costs </strong></td>
					<td align="right" colspan="2"><xsl:value-of select="@TAOH"/></td>
					<td align="right" colspan="2"><xsl:value-of select="@TAMAOH"/></td>
					<td align="right" colspan="2"><xsl:value-of select="@TAOH_INT"/></td>
				</tr>
				<tr>
					<td scope="row" colspan="2"><strong>Total Capital Costs </strong></td>
					<td align="right" colspan="2"><xsl:value-of select="@TCAP"/></td>
					<td align="right" colspan="2"><xsl:value-of select="@TAMCAP"/></td>
					<td align="right" colspan="2"><xsl:value-of select="@TCAP_INT"/></td>
				</tr>
				<tr>
					<td scope="row" colspan="2"><strong>Total Costs - Operation </strong></td>
					<td align="right" colspan="2"><xsl:value-of select="DisplayDevPacks:GetAddNumberN2(@TOC, @TAOH, @TCAP, 0)"/></td>
					<td align="right" colspan="2"><xsl:value-of select="DisplayDevPacks:GetAddNumberN2(@TAMOC, @TAMAOH, @TAMCAP, 0)"/></td>
					<td align="right" colspan="2"><xsl:value-of select="DisplayDevPacks:GetAddNumberN2(@TOC_INT, @TAOH_INT, @TCAP_INT, 0)"/></td>
				</tr>
				<tr>
					<td scope="row" colspan="2"><strong>Total Costs - Operation w. Incentives </strong> </td>
					<td align="right" colspan="2"><xsl:value-of select="@TINCENT"/></td>
					<td align="right" colspan="2"><xsl:value-of select="@TAMINCENT"/></td>
					<td align="right" colspan="2"></td>
				</tr>
				<tr>
					<th scope="col" colspan="8">Inputs</th>
				</tr>
			<xsl:apply-templates select="operationinput">
				<xsl:sort select="@InputDate"/>
			</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="operationinput">
		<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPacks:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
		<tr>
			<th scope="col">
				Input Name
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
				<xsl:value-of select="@InputDate" />
			</td>
			<td>
				<xsl:value-of select="@InputTimes" />
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
				<th scope="col" colspan="2"><strong>Total Costs - Input </strong></th>
				<th scope="col"><strong>Amount</strong></th>
				<th scope="col"><strong>Unit</strong></th>
				<th scope="col"><strong>Price </strong></th>
				<th scope="col"><strong>Total </strong></th>
				<th scope="col"><strong>Interest </strong></th>
				<th scope="col"><strong>Total Cost </strong></th>
			</tr>
			<tr>
				<td colspan="2"><strong>Total Operating Costs </strong></td>
				<td>
          <xsl:value-of select="@InputPrice1Amount" />
				</td>
				<td>
					<xsl:value-of select="@InputUnit1" />
				</td>
				<td>
					<xsl:value-of select="@InputPrice1" />
				</td>
				<td align="right"><xsl:value-of select="@TOC"/></td>
				<td align="right"><xsl:value-of select="@TOC_INT"/></td>
				<td align="right"><xsl:value-of select="DisplayDevPacks:GetAddNumberN2(@TOC, @TOC_INT, 0, 0)"/></td>
			</tr>
			<tr>
				<td colspan="2"><strong>Total Allocated Overhead Costs </strong></td>
				<td>
          <xsl:value-of select="@InputPrice2Amount" />
				</td>
				<td>
					<xsl:value-of select="@InputUnit2" />
				</td>
				<td>
					<xsl:value-of select="@InputPrice2" />
				</td>
				<td align="right"><xsl:value-of select="@TAOH"/></td>
				<td align="right"><xsl:value-of select="@TAOH_INT"/></td>
				<td align="right"><xsl:value-of select="DisplayDevPacks:GetAddNumberN2(@TAOH, @TAOH_INT, 0, 0)"/></td>
			</tr>
			<tr>
				<td colspan="2"><strong>Total Capital Costs </strong></td>
				<td>
          <xsl:value-of select="@InputPrice3Amount" />
				</td>
				<td>
					<xsl:value-of select="@InputUnit3" />
				</td>
				<td>
					<xsl:value-of select="@InputPrice3" />
				</td>
				<td align="right"><xsl:value-of select="@TCAP"/></td>
				<td align="right"><xsl:value-of select="@TCAP_INT"/></td>
				<td align="right"><xsl:value-of select="DisplayDevPacks:GetAddNumberN2(@TCAP, @TCAP_INT, 0, 0)"/></td>
			</tr>
			<tr>
				<td colspan="2"><strong>Total Costs with Incentives </strong></td>
				<td></td>
				<td></td>
				<td></td>
				<td></td>
				<td></td>
				<td align="right"><xsl:value-of select="@TINCENT"/></td>
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


