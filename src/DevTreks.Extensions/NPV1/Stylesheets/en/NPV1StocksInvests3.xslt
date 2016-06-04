<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2013, December -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Investment"
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
					<xsl:apply-templates select="investmentgroup" />
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
		<xsl:apply-templates select="investmentgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentgroup">
    <tr>
			<th scope="col" colspan="10">
				Investment Group :&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" />
			</th>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvtotal1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="investment">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investment">
		<tr>
			<th scope="col" colspan="10">
				Investment :&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" />
			</th>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvtotal1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="investmenttimeperiod">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmenttimeperiod">
		<tr>
			<th scope="col" colspan="10">Time Period :&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" /></th>
		</tr>
    <xsl:apply-templates select="root/linkedview[@AnalyzerType='npvtotal1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:variable name="outcount" select="count(investmentoutcomes/investmentoutcome)"/>
    <xsl:if test="($outcount > 0)"> 
		  <tr>
			  <th scope="col" colspan="10">Outcomes</th>
		  </tr>
		  <xsl:apply-templates select="investmentoutcomes" />
    </xsl:if>
    <xsl:variable name="incount" select="count(investmentcomponents/investmentcomponent)"/>
    <xsl:if test="($incount > 0)"> 
		  <tr>
			  <th scope="col" colspan="10">Components</th>
		  </tr>
		  <xsl:apply-templates select="investmentcomponents" />
    </xsl:if>
	</xsl:template>
	<xsl:template match="investmentoutcomes">
		<xsl:apply-templates select="investmentoutcome">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentoutcome">
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
    <xsl:apply-templates select="investmentoutput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentoutput">
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
	<xsl:template match="investmentcomponents">
		<xsl:apply-templates select="investmentcomponent">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentcomponent">
		<tr>
			<th scope="col" colspan="10">Component: &#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" /></th>
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
    <xsl:apply-templates select="investmentinput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentinput">
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
		<xsl:if test="($localName != 'investmentinput' and $localName != 'investmentoutput')">
      <xsl:if test="($localName != 'investmentcomponent')">
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
     <xsl:if test="($localName != 'investmentoutcome')">
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
		<xsl:if test="($localName = 'investmentinput')">
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
    <xsl:if test="($localName = 'investmentoutput')">
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
