<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2013, December -->
<xsl:stylesheet version="1.0" 
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:devtreks="http://devtreks.org"
	xmlns:y0="urn:DevTreks-support-schemas:Budget"
	xmlns:DisplayDevPacks="urn:displaydevpacks">
	<xsl:output method="xml" indent="yes" omit-xml-declaration="yes" encoding="UTF-8" />
	<!-- pass in params -->
	<!-- what action is being taken by the server -->
	<xsl:param name="serverActionType" />
	<!-- what other action is being taken by the server -->
	<xsl:param name="serverSubActionType" />
	<!-- is the member viewing this uri the owner? -->
	<xsl:param name="isURIOwningClub" />
	<!-- which node to start with?  -->
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
	<!-- what is the owning club's email? -->
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
					<xsl:apply-templates select="budgetgroup" />
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
		<xsl:apply-templates select="budgetgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetgroup">
			<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPacks:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
			<tr>
				<th scope="col" colspan="8">
					Budget Group : <xsl:value-of select="@Name" />
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
					Budget Type : <xsl:value-of select="DisplayDevPacks:WriteSelectListForTypes($searchurl, @ServiceId, @TypeId, 'TypeId', $viewEditType)"/>
				</td>
			</tr>
      <tr>
        <td scope="row" colspan="4">
          Date : <xsl:value-of select="@Date" />
        </td>
				<td scope="row" colspan="4">
					Last Changed : <xsl:value-of select="@LastChangedDate" />
				</td>
			</tr>
			<xsl:apply-templates select="budget">
				<xsl:sort select="@Name"/>
			</xsl:apply-templates>
      <xsl:if test="(@TAMNET != 'NaN')">
			  <tr>
				  <th scope="row" colspan="4">
					  Budget Group Totals and Nets 
				  </th>
				  <th colspan="2">
					  Totals
				  </th>
				  <th colspan="2">
					  Annual Totals
				  </th>
			  </tr>
			  <!-- these are discounted totals: don't subtract out interest-->
			  <tr>
				  <td scope="row" colspan="4">Total Revenue - Budget Group</td>
				  <td class="Content150" colspan="2"><xsl:value-of select="@TR"/></td>
				  <td class="Content150" colspan="2"><xsl:value-of select="@TAMR"/></td>
			  </tr>
				  <tr>
					  <td scope="row" colspan="4">Total Operating Costs - Budget Group</td>
					  <td class="Content150" colspan="2"><xsl:value-of select="@TOC"/></td>
					  <td class="Content150" colspan="2"><xsl:value-of select="@TAMOC"/></td>
				  </tr>
				  <tr>
					  <td scope="row" colspan="4">Total Overhead Costs - Budget Group</td>
					  <td class="Content150" colspan="2"><xsl:value-of select="@TAOH"/></td>
					  <td class="Content150" colspan="2"><xsl:value-of select="@TAMAOH"/></td>
				  </tr>
				  <tr>
					  <td scope="row" colspan="4">Total Capital Costs - Budget Group</td>
					  <td class="Content150" colspan="2"><xsl:value-of select="@TCAP"/></td>
					  <td class="Content150" colspan="2"><xsl:value-of select="@TAMCAP"/></td>
				  </tr>
			  <tr>
				  <td scope="row" colspan="4">Net Profits - Budget Group</td>
				  <td class="Content150" colspan="2"><xsl:value-of select="DisplayDevPacks:GetSubtractNumberN2(@TR, @TOC, @TAOH, @TCAP)"/></td>
				  <td class="Content150" colspan="2"><xsl:value-of select="@TAMNET"/></td>
			  </tr>
        <tr>
				  <td scope="row" colspan="4">Total Incentives Ben -Budget Group</td>
				  <td class="Content150" colspan="2"><xsl:value-of select="@TRINCENT"/></td>
				  <td class="Content150" colspan="2"><xsl:value-of select="@TAMRINCENT"/></td>
			  </tr>
			  <tr>
				  <td scope="row" colspan="4">Total Incentive Costs - Budget Group</td>
				  <td class="Content150" colspan="2"><xsl:value-of select="@TINCENT"/></td>
				  <td class="Content150" colspan="2"><xsl:value-of select="@TAMINCENT"/></td>
			  </tr>
			  <tr>
				  <td scope="row" colspan="4">Net Incentive Profits - Budget Group</td>
				  <td class="Content150" colspan="2"><xsl:value-of select="DisplayDevPacks:GetSubtractNumberN2(@TR, @TINCENT, 0, 0)"/></td>
				  <td class="Content150" colspan="2"><xsl:value-of select="@TAMINCENT_NET"/></td>
			  </tr>
    </xsl:if>
	</xsl:template>
	<xsl:template match="budget">
		<tr>
				<th scope="col" colspan="8">
					Budget :<xsl:value-of select="@Name" />
				</th>
			</tr>
			<tr>
				<th scope="col">
					Label 1
				</th>
				<th>
					Label 2
				</th>
				<th>
					Last&#xA0;Modif.
				</th>
				<th>
					Init. Value
				</th>
				<th>
					Salv. Value
				</th>
				<th>
					Interest Rates
				</th>
				<th>Nom. Rate</th>
				<th>Real Rate</th>
			</tr>
			<tr>
				<td scope="row">
					<xsl:value-of select="@Num" />
				</td>
				<td>
					<xsl:value-of select="@Num2" />
				</td>
				<td>
					<xsl:value-of select="@LastChangedDate" />
				</td>
				<td>
					<xsl:value-of select="@InitialValue" />
				</td>
				<td>
					<xsl:value-of select="@SalvageValue" />
				</td>
				<td></td>
				<td>
          <xsl:value-of select="@NominalRate" />
				</td>
				<td>
					<xsl:value-of select="@RealRate" />
				</td>
			</tr>
			<tr>
				<td colspan="8">
					<strong>Description</strong>
					<br />
					<xsl:value-of select="@Description" />
				</td>
			</tr>
			<xsl:apply-templates select="budgettimeperiod">
				<xsl:sort select="@Date"/>
			</xsl:apply-templates>
      <xsl:if test="(@TAMNET != 'NaN')">
			  <tr>
				  <th scope="row" colspan="4">
					  Budget Totals and Nets
				  </th>
				  <th colspan="2">
					  Totals
				  </th>
				  <th colspan="2">
					  Annual Totals
				  </th>
			  </tr>
			  <tr>
				  <td colspan="4">Total Revenue -Budget</td>
				  <td class="Content150" colspan="2"><xsl:value-of select="@TR"/></td>
				  <td class="Content150" colspan="2"><xsl:value-of select="@TAMR"/></td>
			  </tr>
				  <tr>
					  <td colspan="4">Total Operating Costs -Budget</td>
					  <td class="Content150" colspan="2"><xsl:value-of select="@TOC"/></td>
					  <td class="Content150" colspan="2"><xsl:value-of select="@TAMOC"/></td>
				  </tr>
				  <tr>
					  <td scope="row" colspan="4">Net Operating Profits -Budget</td>
					  <td class="Content150" colspan="2"><xsl:value-of select="DisplayDevPacks:GetSubtractNumberN2(@TR, @TOC, 0, 0)"/></td>
					  <td class="Content150" colspan="2"><xsl:value-of select="DisplayDevPacks:GetSubtractNumberN2(@TAMR, @TAMOC, 0, 0)"/></td>
				  </tr>
				  <tr>
					  <td colspan="4">Total Allocated Overhead Costs -Budget</td>
					  <td class="Content150" colspan="2"><xsl:value-of select="@TAOH"/></td>
					  <td class="Content150" colspan="2"><xsl:value-of select="@TAMAOH"/></td>
				  </tr>
				  <tr >
					  <td colspan="4">Net Operating and Overhead Profits -Budget</td>
					  <td class="Content150" colspan="2"><xsl:value-of select="DisplayDevPacks:GetSubtractNumberN2(@TR, @TOC, @TAOH, 0)"/></td>
					  <td class="Content150" colspan="2"><xsl:value-of select="DisplayDevPacks:GetSubtractNumberN2(@TAMR, @TAMOC, @TAMAOH, 0)"/></td>
				  </tr>
				  <tr>
					  <td colspan="4">Total Capital Expenditure Costs -Budget</td>
					  <td class="Content150" colspan="2"><xsl:value-of select="@TCAP"/></td>
					  <td class="Content150" colspan="2"><xsl:value-of select="@TAMCAP"/></td>
				  </tr>
				  <tr >
					  <td colspan="4">Net Profits -Budget</td>
					  <td class="Content150" colspan="2"><xsl:value-of select="DisplayDevPacks:GetSubtractNumberN2(@TR, @TOC, @TAOH, @TCAP)"/></td>
					  <td class="Content150" colspan="2"><xsl:value-of select="@TAMNET"/></td>
				  </tr>
			  <tr >
				  <td colspan="6">Equivalent Annual Annuity -Budget</td>
				  <td class="Content150" colspan="2"><xsl:value-of select="@InvestmentEAA"/></td>
			  </tr>
        <tr>
				  <td scope="row" colspan="4">Total Incentive Ben -Budget</td>
				  <td class="Content150" colspan="2"><xsl:value-of select="@TRINCENT"/></td>
				  <td class="Content150" colspan="2"><xsl:value-of select="@TAMRINCENT"/></td>
			  </tr>
			  <tr>
				  <td colspan="4">Total Incentive Costs -Budget</td>
				  <td class="Content150" colspan="2"><xsl:value-of select="@TINCENT"/></td>
				  <td class="Content150" colspan="2"><xsl:value-of select="@TAMINCENT"/></td>
			  </tr>
			  <tr>
				  <td colspan="4">Net Incentive Profits -Budget</td>
				  <td class="Content150" colspan="2"><xsl:value-of select="DisplayDevPacks:GetSubtractNumberN2(@TR, @TINCENT, 0, 0)"/></td>
				  <td class="Content150" colspan="2"><xsl:value-of select="@TAMINCENT_NET"/></td>
			  </tr>
      </xsl:if>
	</xsl:template>
	<xsl:template match="budgettimeperiod">
	<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPacks:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
	<tr>
		<th scope="col" colspan="8">
			Time Period : <strong><xsl:value-of select="@EnterpriseName" /></strong>
		</th>
	</tr>
	<tr>
		<th>
			Ending Date
		</th>
		<th>
			Common Ref.?
		</th>
		<th>
			Discount?
		</th>
		<th>
			Enterprise Amt.
		</th>
		<th colspan="2">
			Enter. Unit
		</th>
		<th>
			Growth Type
		</th>
		<th>
			Growth Periods
		</th>
	</tr>
	<tr>
		<td>
			<xsl:value-of select="@Date" />
		</td>
		<td>
      <xsl:value-of select="@CommonRefYorN" />
		</td>
		<td>
      <xsl:value-of select="@DiscountYorN" />
		</td>
		<td>
      <xsl:value-of select="@EnterpriseAmount" />
		</td>
		<td colspan="2">
			<xsl:value-of select="@EnterpriseUnit"/>
		</td>
		<td>
      <xsl:value-of select="@GrowthTypeId"/>
		</td>
		<td>
			<xsl:value-of select="@GrowthPeriods" />
		</td>
	</tr>
	<tr>
		<td scope="row" colspan="1">
			<strong>Time Period</strong>
		</td>
		<td colspan="5">
			<xsl:value-of select="@Name" />
		</td>
		<td colspan="1">
			<strong>Last Changed </strong>
		</td>
		<td colspan="1">
			<xsl:value-of select="@LastChangedDate" />
		</td>
	</tr>
	<tr>
		<td colspan="8">
			<strong>Description</strong>
			<br />
			<xsl:value-of select="@Description" />
		</td>
	</tr>
		<tr>
			<td scope="row" colspan="1">
				<strong>Label</strong>
			</td>
			<td colspan="1">
				<xsl:value-of select="@Num" />
			</td>
			<td colspan="1">
				<strong>Incentive Amount</strong>
			</td>
			<td>
        <xsl:value-of select="@IncentiveAmount" />
			</td>
			<td colspan="1">
				<strong>Incentive Rate</strong>
			</td>
			<td>
        <xsl:value-of select="@IncentiveRate" />
			</td>
			<td colspan="1">
				<strong>Overhead Factor</strong>
			</td>
			<td>
				<xsl:value-of select="@AOHFactor" />
			</td>
		</tr>
		<tr>
			<th scope="col" colspan="8">Revenues</th>
		</tr>
		<xsl:apply-templates select="budgetoutcomes" />
			<tr>
				<td scope="row" colspan="4"></td>
				<td colspan="2">
					Totals
				</td>
				<td colspan="2">
					Annual Totals
				</td>
			</tr>
			<tr>
				<td scope="row" colspan="4">Total Revenue -Time Period</td>
				<td class="Content150" colspan="2"><xsl:value-of select="@TR"/></td>
				<td class="Content150" colspan="2"><xsl:value-of select="@TAMR"/></td>
			</tr>
      <tr>
				<td scope="row" colspan="4">Total Incentive Ben -Time Period</td>
				<td class="Content150" colspan="2"><xsl:value-of select="@TRINCENT"/></td>
				<td class="Content150" colspan="2"><xsl:value-of select="@TAMRINCENT"/></td>
			</tr>
		<tr>
			<th scope="col" colspan="8">Costs</th>
		</tr>
		<xsl:apply-templates select="budgetoperations">
		</xsl:apply-templates>
    <tr>
			<th scope="col" colspan="8">Operating Costs (OC)</th>
		</tr>
		<tr>
			<td scope="row" colspan="4">Total Operating Costs -Time Period</td>
			<td class="Content150" colspan="2"><xsl:value-of select="@TOC"/></td>
			<td class="Content150" colspan="2"><xsl:value-of select="@TAMOC"/></td>
		</tr>
		<tr>
			<td scope="row" colspan="4">Net Operating Profits -Time Period</td>
			<td class="Content150" colspan="2"><xsl:value-of select="DisplayDevPacks:GetSubtractNumberN2(@TR, @TOC, 0, 0)"/></td>
			<td class="Content150" colspan="2"><xsl:value-of select="DisplayDevPacks:GetSubtractNumberN2(@TAMR, @TAMOC, 0, 0)"/></td>
		</tr>
		<tr>
			<th scope="col" colspan="8">Allocated Overhead Costs (AOH)</th>
		</tr>
		<tr>
			<td scope="row" colspan="4">Total Allocated Overhead Costs -Time Period</td>
			<td class="Content150" colspan="2"><xsl:value-of select="@TAOH"/></td>
			<td class="Content150" colspan="2"><xsl:value-of select="@TAMAOH"/></td>
		</tr>
		<tr>
			<td scope="row" colspan="4">Net Operating and Overhead Profits -Time Period</td>
			<td class="Content150" colspan="2"><xsl:value-of select="DisplayDevPacks:GetSubtractNumberN2(@TR, @TOC, @TAOH, 0)"/></td>
			<td class="Content150" colspan="2"><xsl:value-of select="DisplayDevPacks:GetSubtractNumberN2(@TAMR, @TAMOC, @TAMAOH, 0)"/></td>
		</tr>
		<tr>
			<th scope="col" colspan="8">Capital Costs (CAP)</th>
		</tr>
		<tr>
			<td scope="row" colspan="4">Total Capital Costs -Time Period</td>
			<td class="Content150" colspan="2"><xsl:value-of select="@TCAP"/></td>
			<td class="Content150" colspan="2"><xsl:value-of select="@TAMCAP"/></td>
		</tr>
		<tr>
			<td scope="row" colspan="4">Net Profits -Time Period</td>
			<td class="Content150" colspan="2"><xsl:value-of select="DisplayDevPacks:GetSubtractNumberN2(@TR, @TOC, @TAOH, @TCAP)"/></td>
			<td class="Content150" colspan="2"><xsl:value-of select="@TAMNET"/></td>
		</tr>
		<tr>
			<th scope="col" colspan="8">Incentive Costs (INCENT)</th>
		</tr>
			<tr>
				<td scope="row" colspan="4">Total Incentive Costs -Time Period</td>
				<td class="Content150" colspan="2"><xsl:value-of select="@TINCENT"/></td>
				<td class="Content150" colspan="2"><xsl:value-of select="@TAMINCENT"/></td>
			</tr>
			<tr>
				<td scope="row" colspan="4">Net Incentive Profits -Time Period</td>
				<td class="Content150" colspan="2"><xsl:value-of select="DisplayDevPacks:GetSubtractNumberN2(@TR, @TINCENT, 0, 0)"/></td>
				<td class="Content150" colspan="2"><xsl:value-of select="@TAMINCENT_NET"/></td>
			</tr>
	</xsl:template>
  <xsl:template match="budgetoutcomes">
		<xsl:apply-templates select="budgetoutcome">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoutcome">
		<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPacks:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
    <tr>
      <th scope="col" colspan="8">
        Outcome : <xsl:value-of select="@Name" />
      </th>
    </tr>
    <tr>
		<th>
			Ending Date
		</th>
		<th>
			Label
		</th>
		<th>
			Amount
		</th>
		<th>
			Unit
		</th>
		<th>
			Life
		</th>
		<th>
			SalvageValue
		</th>
		<th>
			Incent Amount
		</th>
    <th>
			Incent Rate
		</th>
	</tr>
	<tr>
		<td>
			<xsl:value-of select="@Date" />
		</td>
		<td>
      <xsl:value-of select="@Num" />
		</td>
		<td>
      <xsl:value-of select="@Amount" />
		</td>
		<td>
      <xsl:value-of select="@Unit" />
		</td>
		<td>
			<xsl:value-of select="@EffectiveLife"/>
		</td>
		<td>
      <xsl:value-of select="@SalvageValue"/>
		</td>
		<td>
			<xsl:value-of select="@IncentiveAmount" />
		</td>
    <td>
			<xsl:value-of select="@IncentiveRate" />
		</td>
	</tr>
  <tr>
    <td colspan="8">
      <strong>
        Description
      </strong>
      <br />
      <xsl:value-of select="@Description" />
    </td>
  </tr>
    <tr>
		  <th scope="col">Times</th>
		  <th>Compos Amount</th>
		  <th>Compos Unit</th>
		  <th>Output Amount</th>
		  <th>Output Unit</th>
		  <th>Output Price</th>
		  <th>Total Revenue</th>
		  <th>Annual Total</th>
	  </tr>
    <tr>
      <th scope="col">Date&#xA0;Received</th>
      <th scope="col" colspan="1">Incentive</th>
      <th>
        Incent. Amount
      </th>
      <th>
        Incent. Rate
      </th>
      <th></th>
      <th></th>
      <th>INCENT Total</th>
      <th>Annual Total</th>
    </tr>
    <xsl:apply-templates select="budgetoutput">
      <xsl:sort select="@outputDate"/>
    </xsl:apply-templates>
      <tr>
        <td scope="row" colspan="6">Revenue Interest - Outcome</td>
        <td>
          <xsl:value-of select="@TR_INT"/>
        </td>
        <td>
          <xsl:value-of select="@TAMR_INT"/>
        </td>
      </tr>
    <tr>
      <td scope="row" colspan="6">Total Revenue - Outcome</td>
      <td>
        <xsl:value-of select="@TR"/>
      </td>
      <td>
        <xsl:value-of select="@TAMR"/>
      </td>
    </tr>
    <tr>
		  <td colspan="6">Total Incentive-Adjusted Revenues - Outcome</td>
		  <td><xsl:value-of select="@TRINCENT"/></td>
		  <td><xsl:value-of select="@TAMRINCENT"/></td>
	  </tr>
	</xsl:template>
	<xsl:template match="budgetoutput">
		<tr>
      <th scope="col" colspan="8">
        Output : <xsl:value-of select="@Name" /> Label : <xsl:value-of select="@Num" />
      </th>
    </tr>
		<tr>
			<td scope="row">
        <xsl:value-of select="@OutputTimes" />
			</td>
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
				<xsl:value-of select="@OutputPrice1"/>
			</td>
			<td><xsl:value-of select="@TR"/></td>
			<td></td>
		</tr>
		<tr>
			<td colspan="8">
				<strong>Description</strong>
				<br />
				<xsl:value-of select="@Description" />
			</td>
		</tr>
		<tr>
			<td scope="row" colspan="6">Interest</td>
			<td><xsl:value-of select="@TR_INT"/></td>
			<td></td>
		</tr>
		<tr>
      <td scope="row">
				<xsl:value-of select="@OutputDate"/>
			</td>
			<td colspan="2"><strong>Incentive Amount</strong></td>
			<td>
        <xsl:value-of select="@IncentiveAmount"/>
			</td>
			<td colspan="2"><strong>Incentive Rate</strong></td>
			<td>
        <xsl:value-of select="@IncentiveRate"/>
			</td>
			<td>
				<xsl:value-of select="@TINCENT" />
			</td>
		</tr>
		<tr>
			<td scope="row" colspan="6">Total Output Revenue</td>
			<td>
				<xsl:value-of select="DisplayDevPacks:GetAddNumberN2(@TR, @TR_INT, 0, 0)"/>
			</td>
      <td>
			</td>
		</tr>
	</xsl:template>
	<xsl:template match="budgetoperations">
		<xsl:apply-templates select="budgetoperation">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoperation">
		<tr>
      <th scope="col" colspan="8">
        Operation : <xsl:value-of select="@Name" />
      </th>
    </tr>
    <tr>
		<th>
			Ending Date
		</th>
		<th>
			Label
		</th>
		<th>
			Amount
		</th>
		<th>
			Unit
		</th>
		<th>
			Life
		</th>
		<th>
			SalvageValue
		</th>
		<th>
			Incent Amount
		</th>
    <th>
			Incent Rate
		</th>
	</tr>
	<tr>
		<td>
			<xsl:value-of select="@Date" />
		</td>
		<td>
      <xsl:value-of select="@Num" />
		</td>
		<td>
      <xsl:value-of select="@Amount" />
		</td>
		<td>
      <xsl:value-of select="@Unit" />
		</td>
		<td>
			<xsl:value-of select="@EffectiveLife"/>
		</td>
		<td>
      <xsl:value-of select="@SalvageValue"/>
		</td>
		<td>
			<xsl:value-of select="@IncentiveAmount" />
		</td>
    <td>
			<xsl:value-of select="@IncentiveRate" />
		</td>
	</tr>
  <tr>
    <td colspan="8">
      <strong>
        Description
      </strong>
      <br />
      <xsl:value-of select="@Description" />
    </td>
  </tr>
    <tr>
      <th scope="col">
        Date&#xA0;Applied
      </th>
      <th>
        Times
      </th>
      <th>
        OH Used?
      </th>
      <th>
        Amount
      </th>
      <th>
        Unit
      </th>
      <th>
        Price
      </th>
      <th>Total</th>
      <th>Annual Total</th>
    </tr>
    <tr>
      <th scope="col" colspan="1">Incentive</th>
      <th>
        Incent. Amount
      </th>
      <th>
        Incent. Rate
      </th>
      <th></th>
      <th></th>
      <th></th>
      <th>INCENT Total</th>
      <th>Annual Total</th>
    </tr>
    <xsl:apply-templates select="budgetinput">
      <xsl:sort select="@InputDate"/>
    </xsl:apply-templates>
      <tr>
        <td scope="row" colspan="6">Operating Cost Interest</td>
        <td>
          <xsl:value-of select="@TOC_INT"/>
        </td>
        <td>
          <xsl:value-of select="@TAMOC_INT"/>
        </td>
      </tr>
    <tr>
      <td scope="row" colspan="6">Total Operating Costs - Operation</td>
      <td>
        <xsl:value-of select="@TOC"/>
      </td>
      <td>
        <xsl:value-of select="@TAMOC"/>
      </td>
    </tr>
    <tr>
        <td scope="row" colspan="6">Allocated Overhead Cost Interest</td>
        <td>
          <xsl:value-of select="@TAOH_INT"/>
        </td>
        <td>
          <xsl:value-of select="@TAMAOH_INT"/>
        </td>
      </tr>
    <tr>
			<td colspan="6">Total Allocated Overhead Costs - Operation</td>
			<td><xsl:value-of select="@TAOH"/></td>
			<td><xsl:value-of select="@TAMAOH"/></td>
		</tr>
    <tr>
        <td scope="row" colspan="6">Capital Cost Interest</td>
        <td>
          <xsl:value-of select="@TCAP_INT"/>
        </td>
        <td>
          <xsl:value-of select="@TAMCAP_INT"/>
        </td>
      </tr>
    <tr>
			<td colspan="6">Total Capital Costs - Operation</td>
			<td><xsl:value-of select="@TCAP"/></td>
			<td><xsl:value-of select="@TAMCAP"/></td>
		</tr>
    <tr>
			<td colspan="6">Total Incentive-Adjusted Costs - Operation</td>
			<td><xsl:value-of select="@TINCENT"/></td>
			<td><xsl:value-of select="@TAMINCENT"/></td>
		</tr>
	</xsl:template>
	<xsl:template match="budgetinput">
		<tr>
      <th scope="col" colspan="8">
        Input : <xsl:value-of select="@Name" /> Label : <xsl:value-of select="@Num" />
      </th>
    </tr>
		<tr>
			<td scope="row">
				<xsl:value-of select="@InputDate" />
			</td>
			<td>
				<xsl:value-of select="@InputTimes" />
			</td>
			<td>
        <xsl:value-of select="@InputUseAOHOnly" />
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
			<td><xsl:value-of select="@TOC" /></td>
			<td></td>
		</tr>
    <tr>
			<td colspan="3">Allocated OH</td>
			<td>
        <xsl:value-of select="@InputPrice2Amount" />
			</td>
			<td>
				<xsl:value-of select="@InputUnit2" />
			</td>
			<td>
				<xsl:value-of select="@InputPrice2" />
			</td>
			<td><xsl:value-of select="@TAOH" /></td>
			<td></td>
		</tr>
    <tr>
			<td colspan="3">Capital</td>
			<td>
        <xsl:value-of select="@InputPrice3Amount" />
			</td>
			<td>
				<xsl:value-of select="@InputUnit3" />
			</td>
			<td>
				<xsl:value-of select="@InputPrice3" />
			</td>
			<td><xsl:value-of select="@TCAP" /></td>
			<td></td>
		</tr>
    <tr>
			<td scope="row">Incentive</td>
			<td>
        <xsl:value-of select="@IncentiveAmount" />
			</td>
			<td>
        <xsl:value-of select="@IncentiveRate" />
			</td>
			<td></td>
			<td></td>
			<td></td>
			<td><xsl:value-of select="@TINCENT" /></td>
			<td></td>
		</tr>
		<tr>
			<td colspan="8">
				<strong>Description</strong>
				<br />
				<xsl:value-of select="@Description" />
			</td>
		</tr>
	</xsl:template>
</xsl:stylesheet>