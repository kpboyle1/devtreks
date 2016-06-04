<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2013, October -->
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
      <strong>Input Group</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcaprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="input">
			<xsl:sort select="@InputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="input">
    <h4>
      <strong>Input</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcaprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="inputseries">
			<xsl:sort select="@InputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="inputseries">
		<h4>
      <strong>Input Series</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcaprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
		<xsl:if test="($localName = 'inputgroup')">
      <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Input Group Details</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Date : <xsl:value-of select="@Date"/>
          </div>
          <div class="ui-block-b">
            Observations : <xsl:value-of select="@Observations"/>; Target : <xsl:value-of select="@TargetType"/>
          </div>
          <div class="ui-block-a">
            OC Planned Period : <xsl:value-of select="@TOCCost"/>
          </div>
          <div class="ui-block-b">
            OC Plan Full : <xsl:value-of select="@TOCPFTotal"/>
          </div>
          <div class="ui-block-a">
            OC Plan Cumul : <xsl:value-of select="@TOCPCTotal"/>
          </div>
          <div class="ui-block-b">
            OC Actual Period : <xsl:value-of select="@TOCAPTotal"/>
          </div>
          <div class="ui-block-a">
            OC Actual Cumul : <xsl:value-of select="@TOCACTotal"/>
          </div>
          <div class="ui-block-b">
            OC Actual Period Change : <xsl:value-of select="@TOCAPChange"/>
          </div>
          <div class="ui-block-a">
            OC Actual Cumul Change : <xsl:value-of select="@TOCACChange"/>
          </div>
          <div class="ui-block-b">
            OC Planned Period Percent : <xsl:value-of select="@TOCPPPercent"/>
          </div>
          <div class="ui-block-a">
            OC Planned Cumul Percent : <xsl:value-of select="@TOCPCPercent"/>
          </div>
          <div class="ui-block-b">
            OC Planned Full Percent : <xsl:value-of select="@TOCPFPercent"/>
          </div>
          <div class="ui-block-a">
            AOH Planned Period : <xsl:value-of select="@TAOHCost"/>
          </div>
          <div class="ui-block-b">
            AOH Plan Full : <xsl:value-of select="@TAOHPFTotal"/>
          </div>
          <div class="ui-block-a">
            AOH Plan Cumul : <xsl:value-of select="@TAOHPCTotal"/>
          </div>
          <div class="ui-block-b">
            AOH Actual Period : <xsl:value-of select="@TAOHAPTotal"/>
          </div>
          <div class="ui-block-a">
            AOH Actual Cumul : <xsl:value-of select="@TAOHACTotal"/>
          </div>
          <div class="ui-block-b">
            AOH Actual Period Change : <xsl:value-of select="@TAOHAPChange"/>
          </div>
          <div class="ui-block-a">
            AOH Actual Cumul Change : <xsl:value-of select="@TAOHACChange"/>
          </div>
          <div class="ui-block-b">
            AOH Planned Period Percent : <xsl:value-of select="@TAOHPPPercent"/>
          </div>
          <div class="ui-block-a">
            AOH Planned Cumul Percent : <xsl:value-of select="@TAOHPCPercent"/>
          </div>
          <div class="ui-block-b">
            AOH Planned Full Percent : <xsl:value-of select="@TAOHPFPercent"/>
          </div>
          <div class="ui-block-a">
            CAP Planned Period : <xsl:value-of select="@TCAPCost"/>
          </div>
          <div class="ui-block-b">
            CAP Plan Full : <xsl:value-of select="@TCAPPFTotal"/>
          </div>
          <div class="ui-block-a">
            CAP Plan Cumul : <xsl:value-of select="@TCAPPCTotal"/>
          </div>
          <div class="ui-block-b">
            CAP Actual Period : <xsl:value-of select="@TCAPAPTotal"/>
          </div>
          <div class="ui-block-a">
            CAP Actual Cumul : <xsl:value-of select="@TCAPACTotal"/>
          </div>
          <div class="ui-block-b">
            CAP Actual Period Change : <xsl:value-of select="@TCAPAPChange"/>
          </div>
          <div class="ui-block-a">
            CAP Actual Cumul Change : <xsl:value-of select="@TCAPACChange"/>
          </div>
          <div class="ui-block-b">
            CAP Planned Period Percent : <xsl:value-of select="@TCAPPPPercent"/>
          </div>
          <div class="ui-block-a">
            CAP Planned Cumul Percent : <xsl:value-of select="@TCAPPCPercent"/>
          </div>
          <div class="ui-block-b">
            CAP Planned Full Percent : <xsl:value-of select="@TCAPPFPercent"/>
          </div>
          <div class="ui-block-a">
            LCC Planned Period : <xsl:value-of select="@TLCCCost"/>
          </div>
          <div class="ui-block-b">
            LCC Plan Full : <xsl:value-of select="@TLCCPFTotal"/>
          </div>
          <div class="ui-block-a">
            LCC Plan Cumul : <xsl:value-of select="@TLCCPCTotal"/>
          </div>
          <div class="ui-block-b">
            LCC Actual Period : <xsl:value-of select="@TLCCAPTotal"/>
          </div>
          <div class="ui-block-a">
            LCC Actual Cumul : <xsl:value-of select="@TLCCACTotal"/>
          </div>
          <div class="ui-block-b">
            LCC Actual Period Change : <xsl:value-of select="@TLCCAPChange"/>
          </div>
          <div class="ui-block-a">
            LCC Actual Cumul Change : <xsl:value-of select="@TLCCACChange"/>
          </div>
          <div class="ui-block-b">
            LCC Planned Period Percent : <xsl:value-of select="@TLCCPPPercent"/>
          </div>
          <div class="ui-block-a">
            LCC Planned Cumul Percent : <xsl:value-of select="@TLCCPCPercent"/>
          </div>
          <div class="ui-block-b">
            LCC Planned Full Percent : <xsl:value-of select="@TLCCPFPercent"/>
          </div>
          <div class="ui-block-a">
            EAA Planned Period : <xsl:value-of select="@TEAACost"/>
          </div>
          <div class="ui-block-b">
            EAA Plan Full : <xsl:value-of select="@TEAAPFTotal"/>
          </div>
          <div class="ui-block-a">
            EAA Plan Cumul : <xsl:value-of select="@TEAAPCTotal"/>
          </div>
          <div class="ui-block-b">
            EAA Actual Period : <xsl:value-of select="@TEAAAPTotal"/>
          </div>
          <div class="ui-block-a">
            EAA Actual Cumul : <xsl:value-of select="@TEAAACTotal"/>
          </div>
          <div class="ui-block-b">
            EAA Actual Period Change : <xsl:value-of select="@TEAAAPChange"/>
          </div>
          <div class="ui-block-a">
            EAA Actual Cumul Change : <xsl:value-of select="@TEAAACChange"/>
          </div>
          <div class="ui-block-b">
            EAA Planned Period Percent : <xsl:value-of select="@TEAAPPPercent"/>
          </div>
          <div class="ui-block-a">
            EAA Planned Cumul Percent : <xsl:value-of select="@TEAAPCPercent"/>
          </div>
          <div class="ui-block-b">
            EAA Planned Full Percent : <xsl:value-of select="@TEAAPFPercent"/>
          </div>
          <div class="ui-block-a">
            Unit Planned Period : <xsl:value-of select="@TUnitCost"/>
          </div>
          <div class="ui-block-b">
            Unit Plan Full : <xsl:value-of select="@TUnitPFTotal"/>
          </div>
          <div class="ui-block-a">
            Unit Plan Cumul : <xsl:value-of select="@TUnitPCTotal"/>
          </div>
          <div class="ui-block-b">
            Unit Actual Period : <xsl:value-of select="@TUnitAPTotal"/>
          </div>
          <div class="ui-block-a">
            Unit Actual Cumul : <xsl:value-of select="@TUnitACTotal"/>
          </div>
          <div class="ui-block-b">
            Unit Actual Period Change : <xsl:value-of select="@TUnitAPChange"/>
          </div>
          <div class="ui-block-a">
            Unit Actual Cumul Change : <xsl:value-of select="@TUnitACChange"/>
          </div>
          <div class="ui-block-b">
            Unit Planned Period Percent : <xsl:value-of select="@TUnitPPPercent"/>
          </div>
          <div class="ui-block-a">
            Unit Planned Cumul Percent : <xsl:value-of select="@TUnitPCPercent"/>
          </div>
          <div class="ui-block-b">
            Unit Planned Full Percent : <xsl:value-of select="@TUnitPFPercent"/>
          </div>
        </div>
      </div>
		</xsl:if>
		<xsl:if test="($localName != 'inputgroup')">
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Input Details</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
           Date : <xsl:value-of select="@Date"/>
          </div>
          <div class="ui-block-b">
            Observations : <xsl:value-of select="@Observations"/>; Target : <xsl:value-of select="@TargetType"/>
          </div>
          <div class="ui-block-a">
            OC Planned Period : <xsl:value-of select="@TOCCost"/>
          </div>
          <div class="ui-block-b">
            OC Plan Full : <xsl:value-of select="@TOCPFTotal"/>
          </div>
          <div class="ui-block-a">
            OC Plan Cumul : <xsl:value-of select="@TOCPCTotal"/>
          </div>
          <div class="ui-block-b">
            OC Actual Period : <xsl:value-of select="@TOCAPTotal"/>
          </div>
          <div class="ui-block-a">
            OC Actual Cumul : <xsl:value-of select="@TOCACTotal"/>
          </div>
          <div class="ui-block-b">
            OC Actual Period Change : <xsl:value-of select="@TOCAPChange"/>
          </div>
          <div class="ui-block-a">
            OC Actual Cumul Change : <xsl:value-of select="@TOCACChange"/>
          </div>
          <div class="ui-block-b">
            OC Planned Period Percent : <xsl:value-of select="@TOCPPPercent"/>
          </div>
          <div class="ui-block-a">
            OC Planned Cumul Percent : <xsl:value-of select="@TOCPCPercent"/>
          </div>
          <div class="ui-block-b">
            OC Planned Full Percent : <xsl:value-of select="@TOCPFPercent"/>
          </div>
          <div class="ui-block-a">
            AOH Planned Period : <xsl:value-of select="@TAOHCost"/>
          </div>
          <div class="ui-block-b">
            AOH Plan Full : <xsl:value-of select="@TAOHPFTotal"/>
          </div>
          <div class="ui-block-a">
            AOH Plan Cumul : <xsl:value-of select="@TAOHPCTotal"/>
          </div>
          <div class="ui-block-b">
            AOH Actual Period : <xsl:value-of select="@TAOHAPTotal"/>
          </div>
          <div class="ui-block-a">
            AOH Actual Cumul : <xsl:value-of select="@TAOHACTotal"/>
          </div>
          <div class="ui-block-b">
            AOH Actual Period Change : <xsl:value-of select="@TAOHAPChange"/>
          </div>
          <div class="ui-block-a">
            AOH Actual Cumul Change : <xsl:value-of select="@TAOHACChange"/>
          </div>
          <div class="ui-block-b">
            AOH Planned Period Percent : <xsl:value-of select="@TAOHPPPercent"/>
          </div>
          <div class="ui-block-a">
            AOH Planned Cumul Percent : <xsl:value-of select="@TAOHPCPercent"/>
          </div>
          <div class="ui-block-b">
            AOH Planned Full Percent : <xsl:value-of select="@TAOHPFPercent"/>
          </div>
          <div class="ui-block-a">
            CAP Planned Period : <xsl:value-of select="@TCAPCost"/>
          </div>
          <div class="ui-block-b">
            CAP Plan Full : <xsl:value-of select="@TCAPPFTotal"/>
          </div>
          <div class="ui-block-a">
            CAP Plan Cumul : <xsl:value-of select="@TCAPPCTotal"/>
          </div>
          <div class="ui-block-b">
            CAP Actual Period : <xsl:value-of select="@TCAPAPTotal"/>
          </div>
          <div class="ui-block-a">
            CAP Actual Cumul : <xsl:value-of select="@TCAPACTotal"/>
          </div>
          <div class="ui-block-b">
            CAP Actual Period Change : <xsl:value-of select="@TCAPAPChange"/>
          </div>
          <div class="ui-block-a">
            CAP Actual Cumul Change : <xsl:value-of select="@TCAPACChange"/>
          </div>
          <div class="ui-block-b">
            CAP Planned Period Percent : <xsl:value-of select="@TCAPPPPercent"/>
          </div>
          <div class="ui-block-a">
            CAP Planned Cumul Percent : <xsl:value-of select="@TCAPPCPercent"/>
          </div>
          <div class="ui-block-b">
            CAP Planned Full Percent : <xsl:value-of select="@TCAPPFPercent"/>
          </div>
          <div class="ui-block-a">
            LCC Planned Period : <xsl:value-of select="@TLCCCost"/>
          </div>
          <div class="ui-block-b">
            LCC Plan Full : <xsl:value-of select="@TLCCPFTotal"/>
          </div>
          <div class="ui-block-a">
            LCC Plan Cumul : <xsl:value-of select="@TLCCPCTotal"/>
          </div>
          <div class="ui-block-b">
            LCC Actual Period : <xsl:value-of select="@TLCCAPTotal"/>
          </div>
          <div class="ui-block-a">
            LCC Actual Cumul : <xsl:value-of select="@TLCCACTotal"/>
          </div>
          <div class="ui-block-b">
            LCC Actual Period Change : <xsl:value-of select="@TLCCAPChange"/>
          </div>
          <div class="ui-block-a">
            LCC Actual Cumul Change : <xsl:value-of select="@TLCCACChange"/>
          </div>
          <div class="ui-block-b">
            LCC Planned Period Percent : <xsl:value-of select="@TLCCPPPercent"/>
          </div>
          <div class="ui-block-a">
            LCC Planned Cumul Percent : <xsl:value-of select="@TLCCPCPercent"/>
          </div>
          <div class="ui-block-b">
            LCC Planned Full Percent : <xsl:value-of select="@TLCCPFPercent"/>
          </div>
          <div class="ui-block-a">
            EAA Planned Period : <xsl:value-of select="@TEAACost"/>
          </div>
          <div class="ui-block-b">
            EAA Plan Full : <xsl:value-of select="@TEAAPFTotal"/>
          </div>
          <div class="ui-block-a">
            EAA Plan Cumul : <xsl:value-of select="@TEAAPCTotal"/>
          </div>
          <div class="ui-block-b">
            EAA Actual Period : <xsl:value-of select="@TEAAAPTotal"/>
          </div>
          <div class="ui-block-a">
            EAA Actual Cumul : <xsl:value-of select="@TEAAACTotal"/>
          </div>
          <div class="ui-block-b">
            EAA Actual Period Change : <xsl:value-of select="@TEAAAPChange"/>
          </div>
          <div class="ui-block-a">
            EAA Actual Cumul Change : <xsl:value-of select="@TEAAACChange"/>
          </div>
          <div class="ui-block-b">
            EAA Planned Period Percent : <xsl:value-of select="@TEAAPPPercent"/>
          </div>
          <div class="ui-block-a">
            EAA Planned Cumul Percent : <xsl:value-of select="@TEAAPCPercent"/>
          </div>
          <div class="ui-block-b">
            EAA Planned Full Percent : <xsl:value-of select="@TEAAPFPercent"/>
          </div>
          <div class="ui-block-a">
            Unit Planned Period : <xsl:value-of select="@TUnitCost"/>
          </div>
          <div class="ui-block-b">
            Unit Plan Full : <xsl:value-of select="@TUnitPFTotal"/>
          </div>
          <div class="ui-block-a">
            Unit Plan Cumul : <xsl:value-of select="@TUnitPCTotal"/>
          </div>
          <div class="ui-block-b">
            Unit Actual Period : <xsl:value-of select="@TUnitAPTotal"/>
          </div>
          <div class="ui-block-a">
            Unit Actual Cumul : <xsl:value-of select="@TUnitACTotal"/>
          </div>
          <div class="ui-block-b">
            Unit Actual Period Change : <xsl:value-of select="@TUnitAPChange"/>
          </div>
          <div class="ui-block-a">
            Unit Actual Cumul Change : <xsl:value-of select="@TUnitACChange"/>
          </div>
          <div class="ui-block-b">
            Unit Planned Period Percent : <xsl:value-of select="@TUnitPPPercent"/>
          </div>
          <div class="ui-block-a">
            Unit Planned Cumul Percent : <xsl:value-of select="@TUnitPCPercent"/>
          </div>
          <div class="ui-block-b">
            Unit Planned Full Percent : <xsl:value-of select="@TUnitPFPercent"/>
          </div>
        </div>
      </div>
		</xsl:if>
    <div data-role="collapsible" data-collapsed="true" data-theme="b" data-content-theme="d" >
      <h4 class="ui-bar-b">
        <strong>SubCosts</strong>
      </h4>
      <div class="ui-grid-a">
        <xsl:if test="(@TSubP1Name1 != '' and @TSubP1Name1 != 'none')">
          <div class="ui-block-a">
            SubCost 1 Name :  <xsl:value-of select="@TSubP1Name1"/>
          </div>
          <div class="ui-block-b">
            SubCost 1 Amount : <xsl:value-of select="@TSubP1Amount1"/>
          </div>
          <div class="ui-block-a">
            SubCost 1 Unit :  <xsl:value-of select="@TSubP1Unit1"/>
          </div>
          <div class="ui-block-b">
            SubCost 1 Price : <xsl:value-of select="@TSubP1Price1"/>
          </div>
          <div class="ui-block-a">
            SubCost 1 Total : <xsl:value-of select="@TSubP1Total1"/>
          </div>
          <div class="ui-block-b">
            SubCost 1 Unit Cost : <xsl:value-of select="@TSubP1TotalPerUnit1"/>
          </div>
          <div class="ui-block-a">
            SubCost 1 Description :  <xsl:value-of select="@TSubP1Description1"/>
          </div>
          <div class="ui-block-b">
            SubCost 1 Label :  <xsl:value-of select="@TSubP1Label1"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Name2 != '' and @TSubP1Name2 != 'none')">
          <div class="ui-block-a">
            SubCost 2 Name :  <xsl:value-of select="@TSubP1Name2"/>
          </div>
          <div class="ui-block-b">
            SubCost 2 Amount : <xsl:value-of select="@TSubP1Amount2"/>
          </div>
          <div class="ui-block-a">
            SubCost 2 Unit :  <xsl:value-of select="@TSubP1Unit2"/>
          </div>
          <div class="ui-block-b">
            SubCost 2 Price : <xsl:value-of select="@TSubP1Price2"/>
          </div>
          <div class="ui-block-a">
            SubCost 2 Total : <xsl:value-of select="@TSubP1Total2"/>
          </div>
          <div class="ui-block-b">
            SubCost 2 Unit Cost : <xsl:value-of select="@TSubP1TotalPerUnit2"/>
          </div>
          <div class="ui-block-a">
            SubCost 2 Description :  <xsl:value-of select="@TSubP1Description2"/>
          </div>
          <div class="ui-block-b">
            SubCost 2 Label :  <xsl:value-of select="@TSubP1Label2"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Name3 != '' and @TSubP1Name3 != 'none')">
          <div class="ui-block-a">
            SubCost 3 Name :  <xsl:value-of select="@TSubP1Name3"/>
          </div>
          <div class="ui-block-b">
            SubCost 3 Amount : <xsl:value-of select="@TSubP1Amount3"/>
          </div>
          <div class="ui-block-a">
            SubCost 3 Unit :  <xsl:value-of select="@TSubP1Unit3"/>
          </div>
          <div class="ui-block-b">
            SubCost 3 Price : <xsl:value-of select="@TSubP1Price3"/>
          </div>
          <div class="ui-block-a">
            SubCost 3 Total : <xsl:value-of select="@TSubP1Total3"/>
          </div>
          <div class="ui-block-b">
            SubCost 3 Unit Cost : <xsl:value-of select="@TSubP1TotalPerUnit3"/>
          </div>
          <div class="ui-block-a">
            SubCost 3 Description :  <xsl:value-of select="@TSubP1Description3"/>
          </div>
          <div class="ui-block-b">
            SubCost 3 Label :  <xsl:value-of select="@TSubP1Label3"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Name4 != '' and @TSubP1Name4 != 'none')">
          <div class="ui-block-a">
            SubCost 4 Name :  <xsl:value-of select="@TSubP1Name4"/>
          </div>
          <div class="ui-block-b">
            SubCost 4 Amount : <xsl:value-of select="@TSubP1Amount4"/>
          </div>
          <div class="ui-block-a">
            SubCost 4 Unit :  <xsl:value-of select="@TSubP1Unit4"/>
          </div>
          <div class="ui-block-b">
            SubCost 4 Price : <xsl:value-of select="@TSubP1Price4"/>
          </div>
          <div class="ui-block-a">
            SubCost 4 Total : <xsl:value-of select="@TSubP1Total4"/>
          </div>
          <div class="ui-block-b">
            SubCost 4 Unit Cost : <xsl:value-of select="@TSubP1TotalPerUnit4"/>
          </div>
          <div class="ui-block-a">
            SubCost 4 Description :  <xsl:value-of select="@TSubP1Description4"/>
          </div>
          <div class="ui-block-b">
            SubCost 4 Label :  <xsl:value-of select="@TSubP1Label4"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Name5 != '' and @TSubP1Name5 != 'none')">
          <div class="ui-block-a">
            SubCost 5 Name :  <xsl:value-of select="@TSubP1Name5"/>
          </div>
          <div class="ui-block-b">
            SubCost 5 Amount : <xsl:value-of select="@TSubP1Amount5"/>
          </div>
          <div class="ui-block-a">
            SubCost 5 Unit :  <xsl:value-of select="@TSubP1Unit5"/>
          </div>
          <div class="ui-block-b">
            SubCost 5 Price : <xsl:value-of select="@TSubP1Price5"/>
          </div>
          <div class="ui-block-a">
            SubCost 5 Total : <xsl:value-of select="@TSubP1Total5"/>
          </div>
          <div class="ui-block-b">
            SubCost 5 Unit Cost : <xsl:value-of select="@TSubP1TotalPerUnit5"/>
          </div>
          <div class="ui-block-a">
            SubCost 5 Description :  <xsl:value-of select="@TSubP1Description5"/>
          </div>
          <div class="ui-block-b">
            SubCost 5 Label :  <xsl:value-of select="@TSubP1Label5"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Name6 != '' and @TSubP1Name6 != 'none')">
          <div class="ui-block-a">
            SubCost 6 Name :  <xsl:value-of select="@TSubP1Name6"/>
          </div>
          <div class="ui-block-b">
            SubCost 6 Amount : <xsl:value-of select="@TSubP1Amount6"/>
          </div>
          <div class="ui-block-a">
            SubCost 6 Unit :  <xsl:value-of select="@TSubP1Unit6"/>
          </div>
          <div class="ui-block-b">
            SubCost 6 Price : <xsl:value-of select="@TSubP1Price6"/>
          </div>
          <div class="ui-block-a">
            SubCost 6 Total : <xsl:value-of select="@TSubP1Total6"/>
          </div>
          <div class="ui-block-b">
            SubCost 6 Unit Cost : <xsl:value-of select="@TSubP1TotalPerUnit6"/>
          </div>
          <div class="ui-block-a">
            SubCost 6 Description :  <xsl:value-of select="@TSubP1Description6"/>
          </div>
          <div class="ui-block-b">
            SubCost 6 Label :  <xsl:value-of select="@TSubP1Label6"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Name7 != '' and @TSubP1Name7 != 'none')">
          <div class="ui-block-a">
            SubCost 7 Name :  <xsl:value-of select="@TSubP1Name7"/>
          </div>
          <div class="ui-block-b">
            SubCost 7 Amount : <xsl:value-of select="@TSubP1Amount7"/>
          </div>
          <div class="ui-block-a">
            SubCost 7 Unit :  <xsl:value-of select="@TSubP1Unit7"/>
          </div>
          <div class="ui-block-b">
            SubCost 7 Price : <xsl:value-of select="@TSubP1Price7"/>
          </div>
          <div class="ui-block-a">
            SubCost 7 Total : <xsl:value-of select="@TSubP1Total7"/>
          </div>
          <div class="ui-block-b">
            SubCost 7 Unit Cost : <xsl:value-of select="@TSubP1TotalPerUnit7"/>
          </div>
          <div class="ui-block-a">
            SubCost 7 Description :  <xsl:value-of select="@TSubP1Description7"/>
          </div>
          <div class="ui-block-b">
            SubCost 7 Label :  <xsl:value-of select="@TSubP1Label7"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Name8 != '' and @TSubP1Name8 != 'none')">
          <div class="ui-block-a">
            SubCost 8 Name :  <xsl:value-of select="@TSubP1Name8"/>
          </div>
          <div class="ui-block-b">
            SubCost 8 Amount : <xsl:value-of select="@TSubP1Amount8"/>
          </div>
          <div class="ui-block-a">
            SubCost 8 Unit :  <xsl:value-of select="@TSubP1Unit8"/>
          </div>
          <div class="ui-block-b">
            SubCost 8 Price : <xsl:value-of select="@TSubP1Price8"/>
          </div>
          <div class="ui-block-a">
            SubCost 8 Total : <xsl:value-of select="@TSubP1Total8"/>
          </div>
          <div class="ui-block-b">
            SubCost 8 Unit Cost : <xsl:value-of select="@TSubP1TotalPerUnit8"/>
          </div>
          <div class="ui-block-a">
            SubCost 8 Description :  <xsl:value-of select="@TSubP1Description8"/>
          </div>
          <div class="ui-block-b">
            SubCost 8 Label :  <xsl:value-of select="@TSubP1Label8"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Name9 != '' and @TSubP1Name9 != 'none')">
          <div class="ui-block-a">
            SubCost 9 Name :  <xsl:value-of select="@TSubP1Name9"/>
          </div>
          <div class="ui-block-b">
            SubCost 9 Amount : <xsl:value-of select="@TSubP1Amount9"/>
          </div>
          <div class="ui-block-a">
            SubCost 9 Unit :  <xsl:value-of select="@TSubP1Unit9"/>
          </div>
          <div class="ui-block-b">
            SubCost 9 Price : <xsl:value-of select="@TSubP1Price9"/>
          </div>
          <div class="ui-block-a">
            SubCost 9 Total : <xsl:value-of select="@TSubP1Total9"/>
          </div>
          <div class="ui-block-b">
            SubCost 9 Unit Cost : <xsl:value-of select="@TSubP1TotalPerUnit9"/>
          </div>
          <div class="ui-block-a">
            SubCost 9 Description :  <xsl:value-of select="@TSubP1Description9"/>
          </div>
          <div class="ui-block-b">
            SubCost 9 Label :  <xsl:value-of select="@TSubP1Label9"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Name10 != '' and @TSubP1Name10 != 'none')">
          <div class="ui-block-a">
            SubCost 10 Name :  <xsl:value-of select="@TSubP1Name10"/>
          </div>
          <div class="ui-block-b">
            SubCost 10 Amount : <xsl:value-of select="@TSubP1Amount10"/>
          </div>
          <div class="ui-block-a">
            SubCost 10 Unit :  <xsl:value-of select="@TSubP1Unit10"/>
          </div>
          <div class="ui-block-b">
            SubCost 10 Price : <xsl:value-of select="@TSubP1Price10"/>
          </div>
          <div class="ui-block-a">
            SubCost 10 Total : <xsl:value-of select="@TSubP1Total10"/>
          </div>
          <div class="ui-block-b">
            SubCost 10 Unit Cost : <xsl:value-of select="@TSubP1TotalPerUnit10"/>
          </div>
          <div class="ui-block-a">
            SubCost 10 Description :  <xsl:value-of select="@TSubP1Description10"/>
          </div>
          <div class="ui-block-b">
            SubCost 10 Label :  <xsl:value-of select="@TSubP1Label10"/>
          </div>
        </xsl:if>
      </div>
    </div>
	</xsl:template>
</xsl:stylesheet>
