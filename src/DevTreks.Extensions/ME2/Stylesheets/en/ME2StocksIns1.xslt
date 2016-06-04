<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2014, Jan -->
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
		<div id="modEdits_divEditsDoc">
			<xsl:apply-templates select="servicebase" />
			<xsl:apply-templates select="inputgroup" />
				<div>
					<a id="aFeedback" name="Feedback">
						<xsl:attribute name="href">mailto:<xsl:value-of select="$clubEmail" />?subject=<xsl:value-of select="$selectedFileURIPattern" /></xsl:attribute>
						Feedback About <xsl:value-of select="$selectedFileURIPattern" />
					</a>
        </div>
		</div>
	</xsl:template>
	<xsl:template match="servicebase">
		<h4 class="ui-bar-b">
			Service: <xsl:value-of select="@Name" />
		</h4>
		<xsl:apply-templates select="inputgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="inputgroup">
		<h4>
      <strong>Input Group</strong>: <xsl:value-of select="@Name" />
    </h4>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
    <xsl:choose>
      <xsl:when test="(root/linkedview[@Id=$calculatorid])">
        <xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			    <xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		    </xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="root/linkedview[@AnalyzerType='metotal1']">
			    <xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		    </xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
    <xsl:apply-templates select="input">
			<xsl:sort select="@InputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="input">
    <h4>
      <strong>Input </strong>: <xsl:value-of select="@Name" />
    </h4>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
    <xsl:choose>
      <xsl:when test="(root/linkedview[@Id=$calculatorid])">
        <xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			    <xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		    </xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="root/linkedview[@AnalyzerType='metotal1']">
			    <xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		    </xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
    <xsl:apply-templates select="inputseries">
			<xsl:sort select="@InputDate"/>
		</xsl:apply-templates>
	</xsl:template>
  <xsl:template match="inputseries">
		<h4>
      <strong>Input Series</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
    <xsl:choose>
      <xsl:when test="(root/linkedview[@Id=$calculatorid])">
        <xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			    <xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		    </xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="root/linkedview[@AnalyzerType='metotal1']">
			    <xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		    </xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
    <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
      <h4 class="ui-bar-b">
        <strong>Indicators Details</strong>
      </h4>
      <xsl:if test="(@IndName1 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 1 Name: </strong><xsl:value-of select="@IndName1"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel1"/>
          </div>
          <div class="ui-block-a">
            Weight:  <xsl:value-of select="@IndWeight1"/>
          </div>
          <div class="ui-block-b">
            Date: <xsl:value-of select="@IndDate1"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType1"/>
          </div>
          <div class="ui-block-b">
             Type: <xsl:value-of select="@IndType1"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount1"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit1"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount1"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit1"/>
          </div>
          <div class="ui-block-a">
            Total: <xsl:value-of select="@IndTotal1"/>
          </div>
          <div class="ui-block-b">
            Unit: <xsl:value-of select="@IndUnit1"/>
          </div>
        </div>
        <div>
			    Indic 1 Description: <xsl:value-of select="@IndDescription1" />
	      </div>
      </xsl:if>
      <xsl:if test="(@IndName2 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 2 Name: </strong><xsl:value-of select="@IndName2"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel2"/>
          </div>
          <div class="ui-block-a">
            Weight:  <xsl:value-of select="@IndWeight2"/>
          </div>
          <div class="ui-block-b">
            Date: <xsl:value-of select="@IndDate2"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType2"/>
          </div>
          <div class="ui-block-b">
             Type: <xsl:value-of select="@IndType2"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount2"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit2"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount2"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit2"/>
          </div>
          <div class="ui-block-a">
            Total: <xsl:value-of select="@IndTotal2"/>
          </div>
          <div class="ui-block-b">
            Unit: <xsl:value-of select="@IndUnit2"/>
          </div>
        </div>
        <div>
			    Indic 2 Description: <xsl:value-of select="@IndDescription2" />
	      </div>
      </xsl:if>
      <xsl:if test="(@IndName3 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 3 Name: </strong><xsl:value-of select="@IndName3"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel3"/>
          </div>
          <div class="ui-block-a">
            Weight:  <xsl:value-of select="@IndWeight3"/>
          </div>
          <div class="ui-block-b">
            Date: <xsl:value-of select="@IndDate3"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType3"/>
          </div>
          <div class="ui-block-b">
             Type: <xsl:value-of select="@IndType3"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount3"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit3"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount3"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit3"/>
          </div>
          <div class="ui-block-a">
            Total: <xsl:value-of select="@IndTotal3"/>
          </div>
          <div class="ui-block-b">
            Unit: <xsl:value-of select="@IndUnit3"/>
          </div>
        </div>
        <div>
			    Indic 3 Description: <xsl:value-of select="@IndDescription3" />
	      </div>
      </xsl:if>
      <xsl:if test="(@IndName4 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 4 Name: </strong><xsl:value-of select="@IndName4"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel4"/>
          </div>
          <div class="ui-block-a">
            Weight:  <xsl:value-of select="@IndWeight4"/>
          </div>
          <div class="ui-block-b">
            Date: <xsl:value-of select="@IndDate4"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType4"/>
          </div>
          <div class="ui-block-b">
             Type: <xsl:value-of select="@IndType4"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount4"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit4"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount4"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit4"/>
          </div>
          <div class="ui-block-a">
            Total: <xsl:value-of select="@IndTotal4"/>
          </div>
          <div class="ui-block-b">
            Unit: <xsl:value-of select="@IndUnit4"/>
          </div>
        </div>
        <div>
			    Indic 4 Description: <xsl:value-of select="@IndDescription4" />
	      </div>
      </xsl:if>
      <xsl:if test="(@IndName5 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 5 Name: </strong><xsl:value-of select="@IndName5"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel5"/>
          </div>
          <div class="ui-block-a">
            Weight:  <xsl:value-of select="@IndWeight5"/>
          </div>
          <div class="ui-block-b">
            Date: <xsl:value-of select="@IndDate5"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType5"/>
          </div>
          <div class="ui-block-b">
             Type: <xsl:value-of select="@IndType5"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount5"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit5"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount5"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit5"/>
          </div>
          <div class="ui-block-a">
            Total: <xsl:value-of select="@IndTotal5"/>
          </div>
          <div class="ui-block-b">
            Unit: <xsl:value-of select="@IndUnit5"/>
          </div>
        </div>
        <div>
			    Indic 5 Description: <xsl:value-of select="@IndDescription5" />
	      </div>
      </xsl:if>
      <xsl:if test="(@IndName6 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 6 Name: </strong><xsl:value-of select="@IndName6"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel6"/>
          </div>
          <div class="ui-block-a">
            Weight:  <xsl:value-of select="@IndWeight6"/>
          </div>
          <div class="ui-block-b">
            Date: <xsl:value-of select="@IndDate6"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType6"/>
          </div>
          <div class="ui-block-b">
             Type: <xsl:value-of select="@IndType6"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount6"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit6"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount6"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit6"/>
          </div>
          <div class="ui-block-a">
            Total: <xsl:value-of select="@IndTotal6"/>
          </div>
          <div class="ui-block-b">
            Unit: <xsl:value-of select="@IndUnit6"/>
          </div>
        </div>
        <div>
			    Indic 6 Description: <xsl:value-of select="@IndDescription6" />
	      </div>
      </xsl:if>
      <xsl:if test="(@IndName7 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 7 Name: </strong><xsl:value-of select="@IndName7"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel7"/>
          </div>
          <div class="ui-block-a">
            Weight:  <xsl:value-of select="@IndWeight7"/>
          </div>
          <div class="ui-block-b">
            Date: <xsl:value-of select="@IndDate7"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType7"/>
          </div>
          <div class="ui-block-b">
             Type: <xsl:value-of select="@IndType7"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount7"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit7"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount7"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit7"/>
          </div>
          <div class="ui-block-a">
            Total: <xsl:value-of select="@IndTotal7"/>
          </div>
          <div class="ui-block-b">
            Unit: <xsl:value-of select="@IndUnit7"/>
          </div>
        </div>
        <div>
			    Indic 7 Description: <xsl:value-of select="@IndDescription7" />
	      </div>
      </xsl:if>
      <xsl:if test="(@IndName8 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 8 Name: </strong><xsl:value-of select="@IndName8"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel8"/>
          </div>
          <div class="ui-block-a">
            Weight:  <xsl:value-of select="@IndWeight8"/>
          </div>
          <div class="ui-block-b">
            Date: <xsl:value-of select="@IndDate8"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType8"/>
          </div>
          <div class="ui-block-b">
             Type: <xsl:value-of select="@IndType8"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount8"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit8"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount8"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit8"/>
          </div>
          <div class="ui-block-a">
            Total: <xsl:value-of select="@IndTotal8"/>
          </div>
          <div class="ui-block-b">
            Unit: <xsl:value-of select="@IndUnit8"/>
          </div>
        </div>
        <div>
			    Indic 8 Description: <xsl:value-of select="@IndDescription8" />
	      </div>
      </xsl:if>
      <xsl:if test="(@IndName9 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 9 Name: </strong><xsl:value-of select="@IndName9"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel9"/>
          </div>
          <div class="ui-block-a">
            Weight:  <xsl:value-of select="@IndWeight9"/>
          </div>
          <div class="ui-block-b">
            Date: <xsl:value-of select="@IndDate9"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType9"/>
          </div>
          <div class="ui-block-b">
             Type: <xsl:value-of select="@IndType9"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount9"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit9"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount9"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit9"/>
          </div>
          <div class="ui-block-a">
            Total: <xsl:value-of select="@IndTotal9"/>
          </div>
          <div class="ui-block-b">
            Unit: <xsl:value-of select="@IndUnit9"/>
          </div>
        </div>
        <div>
			    Indic 9 Description: <xsl:value-of select="@IndDescription9" />
	      </div>
      </xsl:if>
      <xsl:if test="(@IndName10 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 10 Name: </strong><xsl:value-of select="@IndName10"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel10"/>
          </div>
          <div class="ui-block-a">
            Weight:  <xsl:value-of select="@IndWeight10"/>
          </div>
          <div class="ui-block-b">
            Date: <xsl:value-of select="@IndDate10"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType10"/>
          </div>
          <div class="ui-block-b">
             Type: <xsl:value-of select="@IndType10"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount10"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit10"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount10"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit10"/>
          </div>
          <div class="ui-block-a">
            Total: <xsl:value-of select="@IndTotal10"/>
          </div>
          <div class="ui-block-b">
            Unit: <xsl:value-of select="@IndUnit10"/>
          </div>
        </div>
        <div>
			    Indic 10 Description: <xsl:value-of select="@IndDescription10" />
	      </div>
      </xsl:if>
      <xsl:if test="(@IndName11 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 11 Name: </strong><xsl:value-of select="@IndName11"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel11"/>
          </div>
          <div class="ui-block-a">
            Weight:  <xsl:value-of select="@IndWeight11"/>
          </div>
          <div class="ui-block-b">
            Date: <xsl:value-of select="@IndDate11"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType11"/>
          </div>
          <div class="ui-block-b">
             Type: <xsl:value-of select="@IndType11"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount11"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit11"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount11"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit11"/>
          </div>
          <div class="ui-block-a">
            Total: <xsl:value-of select="@IndTotal11"/>
          </div>
          <div class="ui-block-b">
            Unit: <xsl:value-of select="@IndUnit11"/>
          </div>
        </div>
        <div>
			    Indic 11 Description: <xsl:value-of select="@IndDescription11" />
	      </div>
      </xsl:if>
      <xsl:if test="(@IndName12 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 12 Name: </strong><xsl:value-of select="@IndName12"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel12"/>
          </div>
          <div class="ui-block-a">
            Weight:  <xsl:value-of select="@IndWeight12"/>
          </div>
          <div class="ui-block-b">
            Date: <xsl:value-of select="@IndDate12"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType12"/>
          </div>
          <div class="ui-block-b">
             Type: <xsl:value-of select="@IndType12"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount12"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit12"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount12"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit12"/>
          </div>
          <div class="ui-block-a">
            Total: <xsl:value-of select="@IndTotal12"/>
          </div>
          <div class="ui-block-b">
            Unit: <xsl:value-of select="@IndUnit12"/>
          </div>
        </div>
        <div>
			    Indic 12 Description: <xsl:value-of select="@IndDescription12" />
	      </div>
      </xsl:if>
      <xsl:if test="(@IndName13 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 13 Name: </strong><xsl:value-of select="@IndName13"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel13"/>
          </div>
          <div class="ui-block-a">
            Weight:  <xsl:value-of select="@IndWeight13"/>
          </div>
          <div class="ui-block-b">
            Date: <xsl:value-of select="@IndDate13"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType13"/>
          </div>
          <div class="ui-block-b">
             Type: <xsl:value-of select="@IndType13"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount13"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit13"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount13"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit13"/>
          </div>
          <div class="ui-block-a">
            Total: <xsl:value-of select="@IndTotal13"/>
          </div>
          <div class="ui-block-b">
            Unit: <xsl:value-of select="@IndUnit13"/>
          </div>
        </div>
        <div>
			    Indic 13 Description: <xsl:value-of select="@IndDescription13" />
	      </div>
      </xsl:if>
      <xsl:if test="(@IndName14 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 14 Name: </strong><xsl:value-of select="@IndName14"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel14"/>
          </div>
          <div class="ui-block-a">
            Weight:  <xsl:value-of select="@IndWeight14"/>
          </div>
          <div class="ui-block-b">
            Date: <xsl:value-of select="@IndDate14"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType14"/>
          </div>
          <div class="ui-block-b">
             Type: <xsl:value-of select="@IndType14"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount14"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit14"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount14"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit14"/>
          </div>
          <div class="ui-block-a">
            Total: <xsl:value-of select="@IndTotal14"/>
          </div>
          <div class="ui-block-b">
            Unit: <xsl:value-of select="@IndUnit14"/>
          </div>
        </div>
        <div>
			    Indic 14 Description: <xsl:value-of select="@IndDescription14" />
	      </div>
      </xsl:if>
      <xsl:if test="(@IndName15 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 15 Name: </strong><xsl:value-of select="@IndName15"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel15"/>
          </div>
          <div class="ui-block-a">
            Weight:  <xsl:value-of select="@IndWeight15"/>
          </div>
          <div class="ui-block-b">
            Date: <xsl:value-of select="@IndDate15"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType15"/>
          </div>
          <div class="ui-block-b">
             Type: <xsl:value-of select="@IndType15"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount15"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit15"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount15"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit15"/>
          </div>
          <div class="ui-block-a">
            Total: <xsl:value-of select="@IndTotal15"/>
          </div>
          <div class="ui-block-b">
            Unit: <xsl:value-of select="@IndUnit15"/>
          </div>
        </div>
        <div>
			    Indic 15 Description: <xsl:value-of select="@IndDescription15" />
	      </div>
      </xsl:if>
      <xsl:if test="(@IndName16 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 16 Name: </strong><xsl:value-of select="@IndName16"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel16"/>
          </div>
          <div class="ui-block-a">
            Weight:  <xsl:value-of select="@IndWeight16"/>
          </div>
          <div class="ui-block-b">
            Date: <xsl:value-of select="@IndDate16"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType16"/>
          </div>
          <div class="ui-block-b">
             Type: <xsl:value-of select="@IndType16"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount16"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit16"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount16"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit16"/>
          </div>
          <div class="ui-block-a">
            Total: <xsl:value-of select="@IndTotal16"/>
          </div>
          <div class="ui-block-b">
            Unit: <xsl:value-of select="@IndUnit16"/>
          </div>
        </div>
        <div>
			    Indic 16 Description: <xsl:value-of select="@IndDescription16" />
	      </div>
      </xsl:if>
      <xsl:if test="(@IndName17 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 17 Name: </strong><xsl:value-of select="@IndName17"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel17"/>
          </div>
          <div class="ui-block-a">
            Weight:  <xsl:value-of select="@IndWeight17"/>
          </div>
          <div class="ui-block-b">
            Date: <xsl:value-of select="@IndDate17"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType17"/>
          </div>
          <div class="ui-block-b">
             Type: <xsl:value-of select="@IndType17"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount17"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit17"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount17"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit17"/>
          </div>
          <div class="ui-block-a">
            Total: <xsl:value-of select="@IndTotal17"/>
          </div>
          <div class="ui-block-b">
            Unit: <xsl:value-of select="@IndUnit17"/>
          </div>
        </div>
        <div>
			    Indic 17 Description: <xsl:value-of select="@IndDescription17" />
	      </div>
      </xsl:if>
      <xsl:if test="(@IndName18 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 18 Name: </strong><xsl:value-of select="@IndName18"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel18"/>
          </div>
          <div class="ui-block-a">
            Weight:  <xsl:value-of select="@IndWeight18"/>
          </div>
          <div class="ui-block-b">
            Date: <xsl:value-of select="@IndDate18"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType18"/>
          </div>
          <div class="ui-block-b">
             Type: <xsl:value-of select="@IndType18"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount18"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit18"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount18"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit18"/>
          </div>
          <div class="ui-block-a">
            Total: <xsl:value-of select="@IndTotal18"/>
          </div>
          <div class="ui-block-b">
            Unit: <xsl:value-of select="@IndUnit18"/>
          </div>
        </div>
        <div>
			    Indic 18 Description: <xsl:value-of select="@IndDescription18" />
	      </div>
      </xsl:if>
      <xsl:if test="(@IndName19 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 19 Name: </strong><xsl:value-of select="@IndName19"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel19"/>
          </div>
          <div class="ui-block-a">
            Weight:  <xsl:value-of select="@IndWeight19"/>
          </div>
          <div class="ui-block-b">
            Date: <xsl:value-of select="@IndDate19"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType19"/>
          </div>
          <div class="ui-block-b">
             Type: <xsl:value-of select="@IndType19"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount19"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit19"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount19"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit19"/>
          </div>
          <div class="ui-block-a">
            Total: <xsl:value-of select="@IndTotal19"/>
          </div>
          <div class="ui-block-b">
            Unit: <xsl:value-of select="@IndUnit19"/>
          </div>
        </div>
        <div>
			    Indic 19 Description: <xsl:value-of select="@IndDescription19" />
	      </div>
      </xsl:if>
      <xsl:if test="(@IndName20 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            <strong>Indic 20 Name: </strong><xsl:value-of select="@IndName20"/>
          </div>
          <div class="ui-block-b">
            Label:  <xsl:value-of select="@IndLabel20"/>
          </div>
          <div class="ui-block-a">
            Weight:  <xsl:value-of select="@IndWeight20"/>
          </div>
          <div class="ui-block-b">
            Date: <xsl:value-of select="@IndDate20"/>
          </div>
          <div class="ui-block-a">
            Math Type:  <xsl:value-of select="@IndMathType20"/>
          </div>
          <div class="ui-block-b">
             Type: <xsl:value-of select="@IndType20"/>
          </div>
          <div class="ui-block-a">
            Q1 Amount:  <xsl:value-of select="@Ind1Amount20"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit: <xsl:value-of select="@Ind1Unit20"/>
          </div>
          <div class="ui-block-a">
            Q2 Amount:  <xsl:value-of select="@Ind2Amount20"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit: <xsl:value-of select="@Ind2Unit20"/>
          </div>
          <div class="ui-block-a">
            Total: <xsl:value-of select="@IndTotal20"/>
          </div>
          <div class="ui-block-b">
            Unit: <xsl:value-of select="@IndUnit20"/>
          </div>
        </div>
        <div>
			    Indic 20 Description: <xsl:value-of select="@IndDescription20" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TotalME2Type1 != '' and @TotalME2Type1 != 'none')">
        <div>
			    M and E Type: <strong><xsl:value-of select="@TotalME2Type1"/></strong>
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name1 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Indicator 1 Name : <strong><xsl:value-of select="@TME2Name1"/></strong>
          </div>
          <div class="ui-block-b">
            Label : <xsl:value-of select="@TME2Label1"/>
          </div>
          <div class="ui-block-a">
            Total : <xsl:value-of select="@TME2Total1"/>
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@TME2Unit1"/>
          </div>
          <div class="ui-block-a">
            Q1 Total : <xsl:value-of select="@TME2Q1Total1"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit : <xsl:value-of select="@TME2Q1Unit1"/>    
          </div>
          <div class="ui-block-a">
            Q2 Total : <xsl:value-of select="@TME2Q2Total1"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit : <xsl:value-of select="@TME2Q2Unit1"/>    
          </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description1" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name2 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Indicator 2 Name : <strong><xsl:value-of select="@TME2Name2"/></strong>
          </div>
          <div class="ui-block-b">
            Label : <xsl:value-of select="@TME2Label2"/>
          </div>
          <div class="ui-block-a">
            Total : <xsl:value-of select="@TME2Total2"/>
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@TME2Unit2"/>
          </div>
          <div class="ui-block-a">
            Q1 Total : <xsl:value-of select="@TME2Q1Total2"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit : <xsl:value-of select="@TME2Q1Unit2"/>    
          </div>
          <div class="ui-block-a">
            Q2 Total : <xsl:value-of select="@TME2Q2Total2"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit : <xsl:value-of select="@TME2Q2Unit2"/>    
          </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description2" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name3 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Indicator 3 Name : <strong><xsl:value-of select="@TME2Name3"/></strong>
          </div>
          <div class="ui-block-b">
            Label : <xsl:value-of select="@TME2Label3"/>
          </div>
          <div class="ui-block-a">
            Total : <xsl:value-of select="@TME2Total3"/>
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@TME2Unit3"/>
          </div>
          <div class="ui-block-a">
            Q1 Total : <xsl:value-of select="@TME2Q1Total3"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit : <xsl:value-of select="@TME2Q1Unit3"/>    
          </div>
          <div class="ui-block-a">
            Q2 Total : <xsl:value-of select="@TME2Q2Total3"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit : <xsl:value-of select="@TME2Q2Unit3"/>    
          </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description3" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name4 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Indicator 4 Name : <strong><xsl:value-of select="@TME2Name4"/></strong>
          </div>
          <div class="ui-block-b">
            Label : <xsl:value-of select="@TME2Label4"/>
          </div>
          <div class="ui-block-a">
            Total : <xsl:value-of select="@TME2Total4"/>
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@TME2Unit4"/>
          </div>
          <div class="ui-block-a">
            Q1 Total : <xsl:value-of select="@TME2Q1Total4"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit : <xsl:value-of select="@TME2Q1Unit4"/>    
          </div>
          <div class="ui-block-a">
            Q2 Total : <xsl:value-of select="@TME2Q2Total4"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit : <xsl:value-of select="@TME2Q2Unit4"/>    
          </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description4" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name5 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Indicator 5 Name : <strong><xsl:value-of select="@TME2Name5"/></strong>
          </div>
          <div class="ui-block-b">
            Label : <xsl:value-of select="@TME2Label5"/>
          </div>
          <div class="ui-block-a">
            Total : <xsl:value-of select="@TME2Total5"/>
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@TME2Unit5"/>
          </div>
          <div class="ui-block-a">
            Q1 Total : <xsl:value-of select="@TME2Q1Total5"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit : <xsl:value-of select="@TME2Q1Unit5"/>    
          </div>
          <div class="ui-block-a">
            Q2 Total : <xsl:value-of select="@TME2Q2Total5"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit : <xsl:value-of select="@TME2Q2Unit5"/>    
          </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description5" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name6 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Indicator 6 Name : <strong><xsl:value-of select="@TME2Name6"/></strong>
          </div>
          <div class="ui-block-b">
            Label : <xsl:value-of select="@TME2Label6"/>
          </div>
          <div class="ui-block-a">
            Total : <xsl:value-of select="@TME2Total6"/>
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@TME2Unit6"/>
          </div>
          <div class="ui-block-a">
            Q1 Total : <xsl:value-of select="@TME2Q1Total6"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit : <xsl:value-of select="@TME2Q1Unit6"/>    
          </div>
          <div class="ui-block-a">
            Q2 Total : <xsl:value-of select="@TME2Q2Total6"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit : <xsl:value-of select="@TME2Q2Unit6"/>    
          </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description6" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name7 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Indicator 7 Name : <strong><xsl:value-of select="@TME2Name7"/></strong>
          </div>
          <div class="ui-block-b">
            Label : <xsl:value-of select="@TME2Label7"/>
          </div>
          <div class="ui-block-a">
            Total : <xsl:value-of select="@TME2Total7"/>
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@TME2Unit7"/>
          </div>
          <div class="ui-block-a">
            Q1 Total : <xsl:value-of select="@TME2Q1Total7"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit : <xsl:value-of select="@TME2Q1Unit7"/>    
          </div>
          <div class="ui-block-a">
            Q2 Total : <xsl:value-of select="@TME2Q2Total7"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit : <xsl:value-of select="@TME2Q2Unit7"/>    
          </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description7" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name8 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Indicator 8 Name : <strong><xsl:value-of select="@TME2Name8"/></strong>
          </div>
          <div class="ui-block-b">
            Label : <xsl:value-of select="@TME2Label8"/>
          </div>
          <div class="ui-block-a">
            Total : <xsl:value-of select="@TME2Total8"/>
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@TME2Unit8"/>
          </div>
          <div class="ui-block-a">
            Q1 Total : <xsl:value-of select="@TME2Q1Total8"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit : <xsl:value-of select="@TME2Q1Unit8"/>    
          </div>
          <div class="ui-block-a">
            Q2 Total : <xsl:value-of select="@TME2Q2Total8"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit : <xsl:value-of select="@TME2Q2Unit8"/>    
          </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description8" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name9 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Indicator 9 Name : <strong><xsl:value-of select="@TME2Name9"/></strong>
          </div>
          <div class="ui-block-b">
            Label : <xsl:value-of select="@TME2Label9"/>
          </div>
          <div class="ui-block-a">
            Total : <xsl:value-of select="@TME2Total9"/>
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@TME2Unit9"/>
          </div>
          <div class="ui-block-a">
            Q1 Total : <xsl:value-of select="@TME2Q1Total9"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit : <xsl:value-of select="@TME2Q1Unit9"/>    
          </div>
          <div class="ui-block-a">
            Q2 Total : <xsl:value-of select="@TME2Q2Total9"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit : <xsl:value-of select="@TME2Q2Unit9"/>    
          </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description9" />
	      </div>
      </xsl:if>
      <xsl:if test="(@TME2Name10 != '')">
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Indicator 10 Name : <strong><xsl:value-of select="@TME2Name10"/></strong>
          </div>
          <div class="ui-block-b">
            Label : <xsl:value-of select="@TME2Label10"/>
          </div>
          <div class="ui-block-a">
            Total : <xsl:value-of select="@TME2Total10"/>
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@TME2Unit10"/>
          </div>
          <div class="ui-block-a">
            Q1 Total : <xsl:value-of select="@TME2Q1Total10"/>
          </div>
          <div class="ui-block-b">
            Q1 Unit : <xsl:value-of select="@TME2Q1Unit10"/>    
          </div>
          <div class="ui-block-a">
            Q2 Total : <xsl:value-of select="@TME2Q2Total10"/>
          </div>
          <div class="ui-block-b">
            Q2 Unit : <xsl:value-of select="@TME2Q2Unit10"/>    
          </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description10" />
	      </div>
      </xsl:if>
    </div>
	</xsl:template>
</xsl:stylesheet>