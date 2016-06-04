<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2013, August -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Outcome"
	xmlns:DisplayComps="urn:displaycomps">
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
		<xsl:apply-templates select="outcomegroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outcomegroup">
    <xsl:variable name="fullcolcount" select="@Files + 1"/>
    <tr>
      <xsl:variable name="title">Outcome Group : <xsl:value-of select="@Name" /> ; <xsl:value-of select="@Num_0" /></xsl:variable>
      <xsl:value-of select="DisplayComps:writeStringFullColumnTH($title, $fullcolcount)"/>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcastat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="outcome">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outcome">
    <xsl:variable name="fullcolcount" select="@Files + 1"/>
    <tr>
			<th scope="col">
				Outcome
			</th>
			<xsl:value-of select="DisplayComps:writeTitles($fullcolcount)"/>
		</tr>
    <tr>
			<td scope="row" colspan="1"><strong>Name</strong></td>
			<td>
				<!--Total-->
			</td>
			<xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			<xsl:for-each select="@*">
				<xsl:variable name="att_name" select="name()"/>
				<xsl:variable name="att_value" select="."/>
				<xsl:value-of select="DisplayComps:printValue('Name_', $att_name, $att_value)"/> 
			</xsl:for-each>
			<xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		</tr>
    <tr>
			<td scope="row" colspan="1"><strong>Label</strong></td>
			<td>
			</td>
			<xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			<xsl:for-each select="@*">
				<xsl:variable name="att_name" select="name()"/>
				<xsl:variable name="att_value" select="."/>
				<xsl:value-of select="DisplayComps:printValue('Num_', $att_name, $att_value)"/> 
			</xsl:for-each>
			<xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcastat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
    <xsl:variable name="fullcolcount" select="@Files + 1"/>
		<xsl:if test="($localName != 'outcomeoutput')">
      <tr>
			  <td scope="row" colspan="1"><strong>Observations</strong></td>
			  <td>
				  <xsl:value-of select="@TBenefitN"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TBenefitN_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>Benefit Total </strong></td>
			  <td>
				  <xsl:value-of select="@TRBenefit"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TRBenefit_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>Benefit Mean</strong></td>
			  <td>
				  <xsl:value-of select="@TRMean"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TRMean_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>Benefit Median</strong></td>
			  <td>
				  <xsl:value-of select="@TRMedian"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TRMedian_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>Benefit Variance</strong></td>
			  <td>
				  <xsl:value-of select="@TRVariance"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TRVariance_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>Benefit Std Dev </strong></td>
			  <td>
				  <xsl:value-of select="@TRStandDev"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TRStandDev_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>LCB Total </strong></td>
			  <td>
				  <xsl:value-of select="@TLCBBenefit"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TLCBBenefit_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>LCB Mean</strong></td>
			  <td>
				  <xsl:value-of select="@TLCBMean"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TLCBMean_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>LCB Median</strong></td>
			  <td>
				  <xsl:value-of select="@TLCBMedian"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TLCBMedian_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>LCB Variance</strong></td>
			  <td>
				  <xsl:value-of select="@TLCBVariance"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TLCBVariance_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>LCB Std Dev </strong></td>
			  <td>
				  <xsl:value-of select="@TLCBStandDev"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TLCBStandDev_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>REAA Total </strong></td>
			  <td>
				  <xsl:value-of select="@TREAABenefit"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TREAABenefit_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>REAA Mean</strong></td>
			  <td>
				  <xsl:value-of select="@TREAAMean"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TREAAMean_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>REAA Median</strong></td>
			  <td>
				  <xsl:value-of select="@TREAAMedian"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TREAAMedian_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>REAA Variance</strong></td>
			  <td>
				  <xsl:value-of select="@TREAAVariance"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TREAAVariance_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>REAA Std Dev </strong></td>
			  <td>
				  <xsl:value-of select="@TREAAStandDev"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TREAAStandDev_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>RUnit Total </strong></td>
			  <td>
				  <xsl:value-of select="@TRUnitBenefit"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TRUnitBenefit_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>RUnit Mean</strong></td>
			  <td>
				  <xsl:value-of select="@TRUnitMean"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TRUnitMean_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>RUnit Median</strong></td>
			  <td>
				  <xsl:value-of select="@TRUnitMedian"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TRUnitMedian_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>RUnit Variance</strong></td>
			  <td>
				  <xsl:value-of select="@TRUnitVariance"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TRUnitVariance_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
      <tr>
			  <td scope="row" colspan="1"><strong>RUnit Std Dev </strong></td>
			  <td>
				  <xsl:value-of select="@TRUnitStandDev"/>
			  </td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
					<xsl:value-of select="DisplayComps:printValue('TRUnitStandDev_', $att_name, $att_value)"/> 
			  </xsl:for-each>
			  <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
		  </tr>
		</xsl:if>
    <tr>
      <td colspan="10"><strong>SubBenefit Totals</strong></td>
    </tr>
      <xsl:if test="(@TSubP2Name1_0 != '' and @TSubP2Name1_0 != 'none') or (@TSubP2Name1_1 != '' and @TSubP2Name1_1 != 'none')">
        <tr>
			    <td scope="row" colspan="1"><strong>
          SubBenefit 1 Name 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Name1_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Amount1_0 != '' and @TSubP2Amount1_0 != 'none') or (@TSubP2Amount1_1 != '' and @TSubP2Amount1_1 != 'none')">
        <tr>
			    <td scope="row" colspan="1"><strong>
          SubBenefit 1 Amount 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Amount1_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Unit1_0 != '' and @TSubP2Unit1_0 != 'none') or (@TSubP2Unit1_1 != '' and @TSubP2Unit1_1 != 'none')">
        <tr>
			    <td scope="row" colspan="1"><strong>
          SubBenefit 1 Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Unit1_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Price1_0 != '' and @TSubP2Price1_0 != 'none') or (@TSubP2Price1_1 != '' and @TSubP2Price1_1 != 'none')">
        <tr>
			    <td scope="row" colspan="1"><strong>
          SubBenefit 1 Price 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Price1_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Total1_0 != '' and @TSubP2Total1_0 != 'none') or (@TSubP2Total1_1 != '' and @TSubP2Total1_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 1 Total 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Total1_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2TotalPerUnit1_0 != '' and @TSubP2TotalPerUnit1_0 != 'none') or (@TSubP2TotalPerUnit1_1 != '' and @TSubP2TotalPerUnit1_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 1 Total Per Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2TotalPerUnit1_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Name2_0 != '' and @TSubP2Name2_0 != 'none') or (@TSubP2Name2_1 != '' and @TSubP2Name2_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 2 Name 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Name2_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Amount2_0 != '' and @TSubP2Amount2_0 != 'none') or (@TSubP2Amount2_1 != '' and @TSubP2Amount2_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 2 Amount 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Amount2_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Unit2_0 != '' and @TSubP2Unit2_0 != 'none') or (@TSubP2Unit2_1 != '' and @TSubP2Unit2_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 2 Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Unit2_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Price2_0 != '' and @TSubP2Price2_0 != 'none') or (@TSubP2Price2_1 != '' and @TSubP2Price2_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 2 Price 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Price2_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Total2_0 != '' and @TSubP2Total2_0 != 'none') or (@TSubP2Total2_1 != '' and @TSubP2Total2_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 2 Total 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Total2_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2TotalPerUnit2_0 != '' and @TSubP2TotalPerUnit2_0 != 'none') or (@TSubP2TotalPerUnit2_1 != '' and @TSubP2TotalPerUnit2_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 2 Total Per Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2TotalPerUnit2_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Name3_0 != '' and @TSubP2Name3_0 != 'none') or (@TSubP2Name3_1 != '' and @TSubP2Name3_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 3 Name 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Name3_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Amount3_0 != '' and @TSubP2Amount3_0 != 'none') or (@TSubP2Amount3_1 != '' and @TSubP2Amount3_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 3 Amount 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Amount3_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Unit3_0 != '' and @TSubP2Unit3_0 != 'none') or (@TSubP2Unit3_1 != '' and @TSubP2Unit3_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 3 Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Unit3_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Price3_0 != '' and @TSubP2Price3_0 != 'none') or (@TSubP2Price3_1 != '' and @TSubP2Price3_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 3 Price 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Price3_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Total3_0 != '' and @TSubP2Total3_0 != 'none') or (@TSubP2Total3_1 != '' and @TSubP2Total3_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 3 Total 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Total3_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2TotalPerUnit3_0 != '' and @TSubP2TotalPerUnit3_0 != 'none') or (@TSubP2TotalPerUnit3_1 != '' and @TSubP2TotalPerUnit3_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 3 Total Per Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2TotalPerUnit3_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Name4_0 != '' and @TSubP2Name4_0 != 'none') or (@TSubP2Name4_1 != '' and @TSubP2Name4_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 4 Name 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Name4_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Amount4_0 != '' and @TSubP2Amount4_0 != 'none') or (@TSubP2Amount4_1 != '' and @TSubP2Amount4_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 4 Amount 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Amount4_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Unit4_0 != '' and @TSubP2Unit4_0 != 'none') or (@TSubP2Unit4_1 != '' and @TSubP2Unit4_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 4 Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Unit4_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Price4_0 != '' and @TSubP2Price4_0 != 'none') or (@TSubP2Price4_1 != '' and @TSubP2Price4_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 4 Price 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Price4_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Total4_0 != '' and @TSubP2Total4_0 != 'none') or (@TSubP2Total4_1 != '' and @TSubP2Total4_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 4 Total 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Total4_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2TotalPerUnit4_0 != '' and @TSubP2TotalPerUnit4_0 != 'none') or (@TSubP2TotalPerUnit4_1 != '' and @TSubP2TotalPerUnit4_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 4 Total Per Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2TotalPerUnit4_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Name5_0 != '' and @TSubP2Name5_0 != 'none') or (@TSubP2Name5_1 != '' and @TSubP2Name5_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 5 Name 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Name5_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Amount5_0 != '' and @TSubP2Amount5_0 != 'none') or (@TSubP2Amount5_1 != '' and @TSubP2Amount5_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 5 Amount 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Amount5_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Unit5_0 != '' and @TSubP2Unit5_0 != 'none') or (@TSubP2Unit5_1 != '' and @TSubP2Unit5_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 5 Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Unit5_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Price5_0 != '' and @TSubP2Price5_0 != 'none') or (@TSubP2Price5_1 != '' and @TSubP2Price5_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 5 Price 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Price5_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Total5_0 != '' and @TSubP2Total5_0 != 'none') or (@TSubP2Total5_1 != '' and @TSubP2Total5_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 5 Total 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Total5_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2TotalPerUnit5_0 != '' and @TSubP2TotalPerUnit5_0 != 'none') or (@TSubP2TotalPerUnit5_1 != '' and @TSubP2TotalPerUnit5_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 5 Total Per Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2TotalPerUnit5_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Name6_0 != '' and @TSubP2Name6_0 != 'none') or (@TSubP2Name6_1 != '' and @TSubP2Name6_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 6 Name 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Name6_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Amount6_0 != '' and @TSubP2Amount6_0 != 'none') or (@TSubP2Amount6_1 != '' and @TSubP2Amount6_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 6 Amount 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Amount6_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Unit6_0 != '' and @TSubP2Unit6_0 != 'none') or (@TSubP2Unit6_1 != '' and @TSubP2Unit6_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 6 Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Unit6_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Price6_0 != '' and @TSubP2Price6_0 != 'none') or (@TSubP2Price6_1 != '' and @TSubP2Price6_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 6 Price 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Price6_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Total6_0 != '' and @TSubP2Total6_0 != 'none') or (@TSubP2Total6_1 != '' and @TSubP2Total6_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 6 Total 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Total6_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2TotalPerUnit6_0 != '' and @TSubP2TotalPerUnit6_0 != 'none') or (@TSubP2TotalPerUnit6_1 != '' and @TSubP2TotalPerUnit6_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 6 Total Per Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2TotalPerUnit6_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Name7_0 != '' and @TSubP2Name7_0 != 'none') or (@TSubP2Name7_1 != '' and @TSubP2Name7_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 7 Name 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Name7_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Amount7_0 != '' and @TSubP2Amount7_0 != 'none') or (@TSubP2Amount7_1 != '' and @TSubP2Amount7_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 7 Amount 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Amount7_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Unit7_0 != '' and @TSubP2Unit7_0 != 'none') or (@TSubP2Unit7_1 != '' and @TSubP2Unit7_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 7 Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Unit7_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Price7_0 != '' and @TSubP2Price7_0 != 'none') or (@TSubP2Price7_1 != '' and @TSubP2Price7_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 7 Price 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Price7_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Total7_0 != '' and @TSubP2Total7_0 != 'none') or (@TSubP2Total7_1 != '' and @TSubP2Total7_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 7 Total 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Total7_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2TotalPerUnit7_0 != '' and @TSubP2TotalPerUnit7_0 != 'none') or (@TSubP2TotalPerUnit7_1 != '' and @TSubP2TotalPerUnit7_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 7 Total Per Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2TotalPerUnit8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Name8_0 != '' and @TSubP2Name8_0 != 'none') or (@TSubP2Name8_1 != '' and @TSubP2Name8_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 8 Name 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Name8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Amount8_0 != '' and @TSubP2Amount8_0 != 'none') or (@TSubP2Amount8_1 != '' and @TSubP2Amount8_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 8 Amount 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Amount8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Unit8_0 != '' and @TSubP2Unit8_0 != 'none') or (@TSubP2Unit8_1 != '' and @TSubP2Unit8_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 8 Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Unit8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Price8_0 != '' and @TSubP2Price8_0 != 'none') or (@TSubP2Price8_1 != '' and @TSubP2Price8_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 8 Price 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Price8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Total8_0 != '' and @TSubP2Total8_0 != 'none') or (@TSubP2Total8_1 != '' and @TSubP2Total8_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 8 Total 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Total8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2TotalPerUnit8_0 != '' and @TSubP2TotalPerUnit8_0 != 'none') or (@TSubP2TotalPerUnit8_1 != '' and @TSubP2TotalPerUnit8_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 8 Total Per Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2TotalPerUnit8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Name9_0 != '' and @TSubP2Name9_0 != 'none') or (@TSubP2Name9_1 != '' and @TSubP2Name9_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 9 Name 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Name9_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Amount9_0 != '' and @TSubP2Amount9_0 != 'none') or (@TSubP2Amount9_1 != '' and @TSubP2Amount9_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 9 Amount 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Amount9_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Unit9_0 != '' and @TSubP2Unit9_0 != 'none') or (@TSubP2Unit9_1 != '' and @TSubP2Unit9_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 9 Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Unit9_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Price9_0 != '' and @TSubP2Price9_0 != 'none') or (@TSubP2Price9_1 != '' and @TSubP2Price9_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 9 Price 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Price9_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Total9_0 != '' and @TSubP2Total9_0 != 'none') or (@TSubP2Total9_1 != '' and @TSubP2Total9_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 9 Total 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Total9_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2TotalPerUnit9_0 != '' and @TSubP2TotalPerUnit9_0 != 'none') or (@TSubP2TotalPerUnit9_1 != '' and @TSubP2TotalPerUnit9_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 9 Total Per Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2TotalPerUnit9_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Name10_0 != '' and @TSubP2Name10_0 != 'none') or (@TSubP2Name10_1 != '' and @TSubP2Name10_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 10 Name 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Name10_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Amount10_0 != '' and @TSubP2Amount10_0 != 'none') or (@TSubP2Amount10_1 != '' and @TSubP2Amount10_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 10 Amount 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Amount10_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Unit10_0 != '' and @TSubP2Unit10_0 != 'none') or (@TSubP2Unit10_1 != '' and @TSubP2Unit10_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 10 Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Unit10_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Price10_0 != '' and @TSubP2Price10_0 != 'none') or (@TSubP2Price10_1 != '' and @TSubP2Price10_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 10 Price 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Price10_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Total10_0 != '' and @TSubP2Total10_0 != 'none') or (@TSubP2Total10_1 != '' and @TSubP2Total10_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 10 Total 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2Total10_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2TotalPerUnit10_0 != '' and @TSubP2TotalPerUnit10_0 != 'none') or (@TSubP2TotalPerUnit10_1 != '' and @TSubP2TotalPerUnit10_1 != 'none')">
        <tr><td scope="row" colspan="1"><strong>
          SubBenefit 10 Total Per Unit 
        </strong></td><td></td>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TSubP2TotalPerUnit10_', $att_name, $att_value)"/>
			  </xsl:for-each>
        
			    <xsl:value-of select="DisplayComps:doPrintValues($fullcolcount)"/>
        </tr>
      </xsl:if>
	</xsl:template>
</xsl:stylesheet>
