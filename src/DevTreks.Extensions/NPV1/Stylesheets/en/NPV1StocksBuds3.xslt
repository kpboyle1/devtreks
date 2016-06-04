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
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvtotal1']">
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
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvtotal1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budgettimeperiod">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgettimeperiod">
		<tr>
			<th scope="col" colspan="10">Time Period :&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" /></th>
		</tr>
    <xsl:apply-templates select="root/linkedview[@AnalyzerType='npvtotal1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:variable name="outcount" select="count(budgetoutcomes/budgetoutcome)"/>
    <xsl:if test="($outcount > 0)"> 
		  <tr>
			  <th scope="col" colspan="10">Outcomes</th>
		  </tr>
		  <xsl:apply-templates select="budgetoutcomes" />
    </xsl:if>
    <xsl:variable name="incount" select="count(budgetoperations/budgetoperation)"/>
    <xsl:if test="($incount > 0)"> 
		  <tr>
			  <th scope="col" colspan="10">Operations</th>
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
			<th scope="col" colspan="10">Outcome:&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" /></th>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvtotal1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <tr>
			<td scope="row" colspan="10">
				<strong>Outputs</strong>
			</td>
		</tr>
    <xsl:apply-templates select="budgetoutput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoutput">
    <tr>
        <td colspan="2">
				  <strong>Name</strong>
			  </td>
        <td>
				  <strong>Unit</strong>
			  </td>
			  <td>
				  <strong>Price</strong>
			  </td>
        <td>
				  <strong>Amount</strong>
			  </td>
        <td>
				  <strong>Compos Unit</strong>
			  </td>
        <td>
				  <strong>Compos Amount</strong>
			  </td>
        <td>
				  <strong>Total Benefit</strong>
			  </td>
        <td>
				  <strong>Total Incentive</strong>
			  </td>
        <td>
			  </td>
		  </tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvtotal1']">
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
			<th scope="col" colspan="10">Operation: <strong>&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" /></strong></th>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvtotal1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <tr>
        <td>
				  <strong>TAMOC</strong>
			  </td>
			  <td>
				  <strong>TAMAOH</strong>
			  </td>
			  <td>
				  <strong>TAMCAP</strong>
			  </td>
			  <td>
				  <strong>TAMTOTAL</strong>
			  </td>
        <td>
				  <strong>TAMIncent</strong>
			  </td>
        <td>
				  <strong>TOC P</strong>
			  </td>
        <td>
				  <strong>TOC Q</strong>
			  </td>
        <td>
          <strong>TAOH P and Q</strong>
			  </td>
        <td>
          <strong>TCAP P</strong>
			  </td>
        <td>
          <strong>TCAP Q</strong>
			  </td>
		  </tr>
    <xsl:apply-templates select="budgetinput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetinput">
		<tr>
			<td scope="row" colspan="10">
				<strong>Input :&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvtotal1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
		<xsl:if test="($localName != 'budgetinput' and $localName != 'budgetoutput')">
      <xsl:if test="($localName != 'budgetoperation')">
       <tr>
		      <td scope="col" colspan="10"><strong>Benefits</strong></td>
	    </tr>
     <tr>
        <td colspan="2">
				  <strong>Output Name</strong>
			  </td>
        <td>
				  <strong>Unit</strong>
			  </td>
			  <td>
				  <strong>Price</strong>
			  </td>
        <td>
				  <strong>Amount</strong>
			  </td>
        <td>
				  <strong>Compos Unit</strong>
			  </td>
        <td>
				  <strong>Compos Amount</strong>
			  </td>
        <td>
				  <strong>Total Benefit</strong>
			  </td>
        <td>
				  <strong>Total Incentive</strong>
			  </td>
        <td>
			  </td>
		  </tr>
      <tr>
			  <td colspan="2">
          <xsl:value-of select="@TRName"/>
			  </td>
        <td>
          <xsl:value-of select="@TRUnit"/>
			  </td>
        <td>
          <xsl:value-of select="@TRPrice"/>
			  </td>
        <td>
          <xsl:value-of select="@TRAmount"/>
			  </td>
        <td>
          <xsl:value-of select="@TRCompositionUnit"/>
			  </td>
        <td>
          <xsl:value-of select="@TRCompositionAmount"/>
			  </td>
        <td>
          <xsl:value-of select="@TAMR"/>
			  </td>
        <td>
          <xsl:value-of select="@TAMRINCENT"/>
			  </td>
        <td>
			  </td>
		  </tr>
     </xsl:if>
     <xsl:if test="($localName != 'budgetoutcome')">
      <tr>
		     <td scope="col" colspan="10"><strong>Costs and Nets</strong></td>
	    </tr>
      <tr>
			  <td>
				  <strong>Total OC</strong>
			  </td>
			  <td>
				  <strong>Total AOH</strong>
			  </td>
			  <td>
				  <strong>Total CAP</strong>
			  </td>
			  <td>
				  <strong>Total Cost</strong>
			  </td>
        <td>
				  <strong>Net Returns</strong>
			  </td>
        <td>
				  <strong>Total Incent</strong>
			  </td>
        <td>
				  <strong>Net Incent Returns</strong>
			  </td>
        <td colspan="3">
			  </td>
		  </tr>
      <tr>
			  <td>
				  <xsl:value-of select="@TAMOC"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TAMAOH"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMCAP"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMTOTAL"/>
			  </td>
        <td>
					  <xsl:value-of select="@TAMNET"/>
			  </td>
        <td>
					  <xsl:value-of select="@TAMINCENT"/>
			  </td>
        <td>
					  <xsl:value-of select="@TAMINCENT_NET"/>
			  </td>
        <td colspan="3">
			  </td>
		  </tr>
    </xsl:if>
		</xsl:if>
		<xsl:if test="($localName = 'budgetinput')">
      <tr>
			  <td>
				  <xsl:value-of select="@TAMOC"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TAMAOH"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMCAP"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TAMTOTAL"/>
			  </td>
        <td>
					  <xsl:value-of select="@TAMINCENT"/>
			  </td>
        <td>
				  <xsl:value-of select="@TOCPrice" />
			  </td>
        <td>
				  <xsl:value-of select="@TOCAmount" />
			  </td>
        <td>
					 <xsl:value-of select="@TAOHPrice" />;<xsl:value-of select="@TAOHAmount" />
			  </td>
        <td>
					  <xsl:value-of select="@TCAPPrice" />
			  </td>
        <td>
					  <xsl:value-of select="@TCAPAmount" />
			  </td>
		  </tr>
		</xsl:if>
    <xsl:if test="($localName = 'budgetoutput')">
      <tr>
			  <td colspan="2">
          <xsl:value-of select="@TRName"/>
			  </td>
        <td>
          <xsl:value-of select="@TRUnit"/>
			  </td>
        <td>
          <xsl:value-of select="@TRPrice"/>
			  </td>
        <td>
          <xsl:value-of select="@TRAmount"/>
			  </td>
        <td>
          <xsl:value-of select="@TRCompositionUnit"/>
			  </td>
        <td>
          <xsl:value-of select="@TRCompositionAmount"/>
			  </td>
       <td>
          <xsl:value-of select="@TAMR"/>
			  </td>
        <td>
          <xsl:value-of select="@TAMRINCENT"/>
			  </td>
        <td>
			  </td>
		  </tr>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
