<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2013, September -->
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
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcachangeyr' 
      or @AnalyzerType='lcachangeid' or @AnalyzerType='lcachangealt']">
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
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcachangeyr' 
      or @AnalyzerType='lcachangeid' or @AnalyzerType='lcachangealt']">
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
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcachangeyr' 
      or @AnalyzerType='lcachangeid' or @AnalyzerType='lcachangealt']">
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
            Observations : <xsl:value-of select="@Observations"/>; Alternative : <xsl:value-of select="@AlternativeType"/>
          </div>
          <div class="ui-block-a">
            OC Total : <xsl:value-of select="@TOCCost"/>
          </div>
          <div class="ui-block-b">
            OC AmountChange : <xsl:value-of select="@TOCAmountChange"/>
          </div>
          <div class="ui-block-a">
            OC PercentChange : <xsl:value-of select="@TOCPercentChange"/>
          </div>
          <div class="ui-block-b">
            OC BaseChange : <xsl:value-of select="@TOCBaseChange"/>
          </div>
          <div class="ui-block-a">
            OC BasePercentChange : <xsl:value-of select="@TOCBasePercentChange"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            AOH Total : <xsl:value-of select="@TAOHCost"/>
          </div>
          <div class="ui-block-b">
            AOH AmountChange : <xsl:value-of select="@TAOHAmountChange"/>
          </div>
          <div class="ui-block-a">
            AOH PercentChange : <xsl:value-of select="@TAOHPercentChange"/>
          </div>
          <div class="ui-block-b">
            AOH BaseChange : <xsl:value-of select="@TAOHBaseChange"/>
          </div>
          <div class="ui-block-a">
            AOH BasePercentChange : <xsl:value-of select="@TAOHBasePercentChange"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            CAP Total : <xsl:value-of select="@TCAPCost"/>
          </div>
          <div class="ui-block-b">
            CAP AmountChange : <xsl:value-of select="@TCAPAmountChange"/>
          </div>
          <div class="ui-block-a">
            CAP PercentChange : <xsl:value-of select="@TCAPPercentChange"/>
          </div>
          <div class="ui-block-b">
            CAP BaseChange : <xsl:value-of select="@TCAPBaseChange"/>
          </div>
          <div class="ui-block-a">
            CAP BasePercentChange : <xsl:value-of select="@TCAPBasePercentChange"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            LCC Total : <xsl:value-of select="@TLCCCost"/>
          </div>
          <div class="ui-block-b">
            LCC AmountChange : <xsl:value-of select="@TLCCAmountChange"/>
          </div>
          <div class="ui-block-a">
            LCC PercentChange : <xsl:value-of select="@TLCCPercentChange"/>
          </div>
          <div class="ui-block-b">
            LCC BaseChange : <xsl:value-of select="@TLCCBaseChange"/>
          </div>
          <div class="ui-block-a">
            LCC BasePercentChange : <xsl:value-of select="@TLCCBasePercentChange"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            EAA Total : <xsl:value-of select="@TEAACost"/>
          </div>
          <div class="ui-block-b">
            EAA AmountChange : <xsl:value-of select="@TEAAAmountChange"/>
          </div>
          <div class="ui-block-a">
            EAA PercentChange : <xsl:value-of select="@TEAAPercentChange"/>
          </div>
          <div class="ui-block-b">
            EAA BaseChange : <xsl:value-of select="@TEAABaseChange"/>
          </div>
          <div class="ui-block-a">
            EAA BasePercentChange : <xsl:value-of select="@TEAABasePercentChange"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            Unit Total : <xsl:value-of select="@TUnitCost"/>
          </div>
          <div class="ui-block-b">
            Unit AmountChange : <xsl:value-of select="@TUnitAmountChange"/>
          </div>
          <div class="ui-block-a">
            Unit PercentChange : <xsl:value-of select="@TUnitPercentChange"/>
          </div>
          <div class="ui-block-b">
            Unit BaseChange : <xsl:value-of select="@TUnitBaseChange"/>
          </div>
          <div class="ui-block-a">
            Unit BasePercentChange : <xsl:value-of select="@TUnitBasePercentChange"/>
          </div>
          <div class="ui-block-b">    
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
            Observations : <xsl:value-of select="@Observations"/>; Alternative : <xsl:value-of select="@AlternativeType"/>
          </div>
          <div class="ui-block-a">
            OC Total : <xsl:value-of select="@TOCCost"/>
          </div>
          <div class="ui-block-b">
            OC Amount Change : <xsl:value-of select="@TOCAmountChange"/>
          </div>
          <div class="ui-block-a">
            OC Percent Change : <xsl:value-of select="@TOCPercentChange"/>
          </div>
          <div class="ui-block-b">
            OC Base Change : <xsl:value-of select="@TOCBaseChange"/>
          </div>
          <div class="ui-block-a">
            OC Base Percent Change : <xsl:value-of select="@TOCBasePercentChange"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            AOH Total : <xsl:value-of select="@TAOHCost"/>
          </div>
          <div class="ui-block-b">
            AOH AmountChange : <xsl:value-of select="@TAOHAmountChange"/>
          </div>
          <div class="ui-block-a">
            AOH PercentChange : <xsl:value-of select="@TAOHPercentChange"/>
          </div>
          <div class="ui-block-b">
            AOH BaseChange : <xsl:value-of select="@TAOHBaseChange"/>
          </div>
          <div class="ui-block-a">
            AOH BasePercentChange : <xsl:value-of select="@TAOHBasePercentChange"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            CAP Total : <xsl:value-of select="@TCAPCost"/>
          </div>
          <div class="ui-block-b">
            CAP AmountChange : <xsl:value-of select="@TCAPAmountChange"/>
          </div>
          <div class="ui-block-a">
            CAP PercentChange : <xsl:value-of select="@TCAPPercentChange"/>
          </div>
          <div class="ui-block-b">
            CAP BaseChange : <xsl:value-of select="@TCAPBaseChange"/>
          </div>
          <div class="ui-block-a">
            CAP BasePercentChange : <xsl:value-of select="@TCAPBasePercentChange"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            LCC Total : <xsl:value-of select="@TLCCCost"/>
          </div>
          <div class="ui-block-b">
            LCC AmountChange : <xsl:value-of select="@TLCCAmountChange"/>
          </div>
          <div class="ui-block-a">
            LCC PercentChange : <xsl:value-of select="@TLCCPercentChange"/>
          </div>
          <div class="ui-block-b">
            LCC BaseChange : <xsl:value-of select="@TLCCBaseChange"/>
          </div>
          <div class="ui-block-a">
            LCC BasePercentChange : <xsl:value-of select="@TLCCBasePercentChange"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            EAA Total : <xsl:value-of select="@TEAACost"/>
          </div>
          <div class="ui-block-b">
            EAA AmountChange : <xsl:value-of select="@TEAAAmountChange"/>
          </div>
          <div class="ui-block-a">
            EAA PercentChange : <xsl:value-of select="@TEAAPercentChange"/>
          </div>
          <div class="ui-block-b">
            EAA BaseChange : <xsl:value-of select="@TEAABaseChange"/>
          </div>
          <div class="ui-block-a">
            EAA BasePercentChange : <xsl:value-of select="@TEAABasePercentChange"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            Unit Total : <xsl:value-of select="@TUnitCost"/>
          </div>
          <div class="ui-block-b">
            Unit AmountChange : <xsl:value-of select="@TUnitAmountChange"/>
          </div>
          <div class="ui-block-a">
            Unit PercentChange : <xsl:value-of select="@TUnitPercentChange"/>
          </div>
          <div class="ui-block-b">
            Unit BaseChange : <xsl:value-of select="@TUnitBaseChange"/>
          </div>
          <div class="ui-block-a">
            Unit BasePercentChange : <xsl:value-of select="@TUnitBasePercentChange"/>
          </div>
          <div class="ui-block-b">    
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
