<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2013, December -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
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
					<xsl:apply-templates select="budgetgroup" />
					<tr id="footer">
						<td scope="row" colspan="10">
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
		<tr>
			<th colspan="10">
				Service: <xsl:value-of select="@Name" />
			</th>
		</tr>
		<xsl:apply-templates select="budgetgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetgroup">
    <tr>
			<th scope="col" colspan="10">
				Budget Group :&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" />
			</th>
		</tr>
    <tr>
      <th>
        Total Type
			</th>
      <th>
				Total
			</th>
			<th>
				Mean
			</th>
			<th>
				Median
			</th>
      <th>
				Variance
			</th>
			<th>
				Std Dev
			</th>
      <th colspan="4">
			</th>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvstat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budget">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budget">
		<tr>
			<th scope="col" colspan="10">
				Budget :&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" />
			</th>
		</tr>
    <tr>
      <th>
				Total Type
			</th>
      <th>
				Total
			</th>
			<th>
				Mean
			</th>
			<th>
				Median
			</th>
      <th>
				Variance
			</th>
			<th>
				Std Dev
			</th>
      <th colspan="4">
			</th>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvstat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budgettimeperiod">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgettimeperiod">
		<tr>
			<th scope="col" colspan="10">
				Time Period :&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" />
			</th>
		</tr>
    <tr>
      <th>
				Total Type
			</th>
      <th>
				Total
			</th>
			<th>
				Mean
			</th>
			<th>
				Median
			</th>
      <th>
				Variance
			</th>
			<th>
				Std Dev
			</th>
      <th colspan="4">
			</th>
		</tr>
    <xsl:apply-templates select="root/linkedview[@AnalyzerType='npvstat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:variable name="outcount" select="count(budgetoutcomes/budgetoutcome)"/>
    <xsl:if test="($outcount > 0)"> 
      <tr>
      <th>
				Total Type
			</th>
      <th>
				Total
			</th>
			<th>
				Mean
			</th>
			<th>
				Median
			</th>
      <th>
				Variance
			</th>
			<th>
				Std Dev
			</th>
      <th colspan="4">
			</th>
		</tr>
		  <xsl:apply-templates select="budgetoutcomes" />
    </xsl:if>
    <xsl:variable name="opcount" select="count(budgetoperations/budgetoperation)"/>
    <xsl:if test="($opcount > 0)"> 
      <tr>
      <th>
				Total Type
			</th>
      <th>
				Total
			</th>
			<th>
				Mean
			</th>
			<th>
				Median
			</th>
      <th>
				Variance
			</th>
			<th>
				Std Dev
			</th>
      <th colspan="4">
			</th>
		</tr>
		  <xsl:apply-templates select="budgetoperations" />
    </xsl:if>
	</xsl:template>
	<xsl:template match="budgetoutcomes">
		<xsl:apply-templates select="budgetoutcome">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoutcome">
		<tr>
			<td scope="row" colspan="10">
				<strong>Outcome&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvstat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoperations">
		<xsl:apply-templates select="budgetoperation">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoperation">
		<tr>
			<td scope="row" colspan="10">
				<strong>Operation&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvstat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
      <xsl:if test="($localName != 'budgetoperation')">
		  <tr>
			  <td colspan="10">
				  <strong>Benefit Observations</strong> : <xsl:value-of select="@TBenefitN"/>
			  </td>
      </tr>
			<tr>
			  <td>
				  <strong>Benefits</strong>
			  </td>
			  <td>
				  <xsl:value-of select="@TAMR"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMR_MEAN"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMR_MED"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMR_VAR2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TAMR_SD"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
      <tr>
			  <td>
				  <strong>Incentives</strong>
			  </td>
			  <td>
				  <xsl:value-of select="@TAMRINCENT"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMRINCENT_MEAN"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMRINCENT_MED"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMRINCENT_VAR2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TAMRINCENT_SD"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
      <tr>
        <td>
				  <strong>Amount</strong>
			  </td>
			  <td>
				  <xsl:value-of select="@TRAmount"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TRAmount_MEAN"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TRAmount_MED"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TRAmount_VAR2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TRAmount_SD"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
      <tr>
			  <td>
				  <strong>Composition Amount</strong>
			  </td>
			  <td>
				  <xsl:value-of select="@TRCompositionAmount"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TRCompositionAmount_MEAN"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TRCompositionAmount_MED"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TRCompositionAmount_VAR2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TRCompositionAmount_SD"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
      <tr>
			  <td>
				  <strong>Price</strong>
			  </td>
			  <td>
				  <xsl:value-of select="@TRPrice"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TRPrice_MEAN"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TRPrice_MED"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TRPrice_VAR2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TRPrice_SD"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
     </xsl:if>
     <xsl:if test="($localName != 'budgetoutcome')">
      <tr>
			  <td colspan="10">
				  <strong>Cost Observations</strong> : <xsl:value-of select="@TCostN"/>
			  </td>
      </tr>
			<tr>
			  <td>
				  <strong>OC</strong>
			  </td>
			  <td>
				  <xsl:value-of select="@TAMOC"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMOC_MEAN"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMOC_MED"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMOC_VAR2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TAMOC_SD"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
      <tr>
			  <td>
				  <strong>AOH</strong>
			  </td>
			  <td>
				  <xsl:value-of select="@TAMAOH"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMAOH_MEAN"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMAOH_MED"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMAOH_VAR2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TAMAOH_SD"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
      <tr>
			  <td>
				  <strong>CAP</strong>
			  </td>
			  <td>
				  <xsl:value-of select="@TAMCAP"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMCAP_MEAN"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMCAP_MED"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMCAP_VAR2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TAMCAP_SD"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
      <tr>
			  <td>
				  <strong>Total Cost</strong>
			  </td>
			  <td>
				  <xsl:value-of select="@TAMTOTAL"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMTOTAL_MEAN"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMTOTAL_MED"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMTOTAL_VAR2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TAMTOTAL_SD"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
      <tr>
			  <td>
				  <strong>Net</strong>
			  </td>
			  <td>
				  <xsl:value-of select="@TAMNET"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMNET_MEAN"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMNET_MED"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMNET_VAR2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TAMNET_SD"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
      <tr>
			  <td>
				  <strong>Incentives</strong>
			  </td>
			  <td>
				  <xsl:value-of select="@TAMINCENT"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMINCENT_MEAN"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMINCENT_MED"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMINCENT_VAR2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TAMINCENT_SD"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
      <tr>
			  <td>
				  <strong>Net Incentive</strong>
			  </td>
			  <td>
				  <xsl:value-of select="@TAMINCENT_NET"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMINCENT_NET_MEAN"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMINCENT_NET_MED"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMINCENT_NET_VAR2"/>
			  </td>
        <td>
					  <xsl:value-of select="@TAMINCENT_NET_SD"/>
			  </td>
        <td colspan="4">
			  </td>
		  </tr>
    </xsl:if>
	</xsl:template>
</xsl:stylesheet>
