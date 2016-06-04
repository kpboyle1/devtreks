<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2013, October -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Output"
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
			<xsl:apply-templates select="outputgroup" />
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
		<xsl:apply-templates select="outputgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outputgroup">
		<h4>
      <strong>Output Group</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcachangeyr' 
      or @AnalyzerType='lcachangeid' or @AnalyzerType='lcachangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="output">
			<xsl:sort select="@OutputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="output">
    <h4>
      <strong>Output</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcachangeyr' 
      or @AnalyzerType='lcachangeid' or @AnalyzerType='lcachangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="outputseries">
			<xsl:sort select="@OutputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outputseries">
		<h4>
      <strong>Output Series</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcachangeyr' 
      or @AnalyzerType='lcachangeid' or @AnalyzerType='lcachangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
		<xsl:if test="($localName = 'outputgroup')">
      <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Output Group Details</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Date : <xsl:value-of select="@Date"/>
          </div>
          <div class="ui-block-b">
            Observations : <xsl:value-of select="@Observations"/>; Alternative : <xsl:value-of select="@AlternativeType"/>
          </div>
          <div class="ui-block-a">
            Ben Total : <xsl:value-of select="@TRBenefit"/>
          </div>
          <div class="ui-block-b">
            Ben AmountChange : <xsl:value-of select="@TRAmountChange"/>
          </div>
          <div class="ui-block-a">
            Ben PercentChange : <xsl:value-of select="@TRPercentChange"/>
          </div>
          <div class="ui-block-b">
            Ben BaseChange : <xsl:value-of select="@TRBaseChange"/>
          </div>
          <div class="ui-block-a">
            Ben BasePercentChange : <xsl:value-of select="@TRBasePercentChange"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            LCB Total : <xsl:value-of select="@TLCBBenefit"/>
          </div>
          <div class="ui-block-b">
            LCB AmountChange : <xsl:value-of select="@TLCBAmountChange"/>
          </div>
          <div class="ui-block-a">
            LCB PercentChange : <xsl:value-of select="@TLCBPercentChange"/>
          </div>
          <div class="ui-block-b">
            LCB BaseChange : <xsl:value-of select="@TLCBBaseChange"/>
          </div>
          <div class="ui-block-a">
            LCB BasePercentChange : <xsl:value-of select="@TLCBBasePercentChange"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            REAA Total : <xsl:value-of select="@TREAABenefit"/>
          </div>
          <div class="ui-block-b">
            REAA AmountChange : <xsl:value-of select="@TREAAAmountChange"/>
          </div>
          <div class="ui-block-a">
            REAA PercentChange : <xsl:value-of select="@TREAAPercentChange"/>
          </div>
          <div class="ui-block-b">
            REAA BaseChange : <xsl:value-of select="@TREAABaseChange"/>
          </div>
          <div class="ui-block-a">
            REAA BasePercentChange : <xsl:value-of select="@TREAABasePercentChange"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            Unit Total : <xsl:value-of select="@TRUnitBenefit"/>
          </div>
          <div class="ui-block-b">
            Unit AmountChange : <xsl:value-of select="@TRUnitAmountChange"/>
          </div>
          <div class="ui-block-a">
            Unit PercentChange : <xsl:value-of select="@TRUnitPercentChange"/>
          </div>
          <div class="ui-block-b">
            Unit BaseChange : <xsl:value-of select="@TRUnitBaseChange"/>
          </div>
          <div class="ui-block-a">
            Unit BasePercentChange : <xsl:value-of select="@TRUnitBasePercentChange"/>
          </div>
          <div class="ui-block-b">    
          </div>
        </div>
      </div>
		</xsl:if>
		<xsl:if test="($localName != 'outputgroup')">
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Output Details</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
           Date : <xsl:value-of select="@Date"/>
          </div>
          <div class="ui-block-b">
            Observations : <xsl:value-of select="@Observations"/>; Alternative : <xsl:value-of select="@AlternativeType"/>
          </div>
          <div class="ui-block-a">
            Ben Total : <xsl:value-of select="@TRBenefit"/>
          </div>
          <div class="ui-block-b">
            Ben Amount Change : <xsl:value-of select="@TRAmountChange"/>
          </div>
          <div class="ui-block-a">
            Ben Percent Change : <xsl:value-of select="@TRPercentChange"/>
          </div>
          <div class="ui-block-b">
            Ben Base Change : <xsl:value-of select="@TRBaseChange"/>
          </div>
          <div class="ui-block-a">
            Ben Base Percent Change : <xsl:value-of select="@TRBasePercentChange"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            LCB Total : <xsl:value-of select="@TLCBBenefit"/>
          </div>
          <div class="ui-block-b">
            LCB AmountChange : <xsl:value-of select="@TLCBAmountChange"/>
          </div>
          <div class="ui-block-a">
            LCB PercentChange : <xsl:value-of select="@TLCBPercentChange"/>
          </div>
          <div class="ui-block-b">
            LCB BaseChange : <xsl:value-of select="@TLCBBaseChange"/>
          </div>
          <div class="ui-block-a">
            LCB BasePercentChange : <xsl:value-of select="@TLCBBasePercentChange"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            REAA Total : <xsl:value-of select="@TREAABenefit"/>
          </div>
          <div class="ui-block-b">
            REAA AmountChange : <xsl:value-of select="@TREAAAmountChange"/>
          </div>
          <div class="ui-block-a">
            REAA PercentChange : <xsl:value-of select="@TREAAPercentChange"/>
          </div>
          <div class="ui-block-b">
            REAA BaseChange : <xsl:value-of select="@TREAABaseChange"/>
          </div>
          <div class="ui-block-a">
            REAA BasePercentChange : <xsl:value-of select="@TREAABasePercentChange"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            Unit Total : <xsl:value-of select="@TRUnitBenefit"/>
          </div>
          <div class="ui-block-b">
            Unit AmountChange : <xsl:value-of select="@TRUnitAmountChange"/>
          </div>
          <div class="ui-block-a">
            Unit PercentChange : <xsl:value-of select="@TRUnitPercentChange"/>
          </div>
          <div class="ui-block-b">
            Unit BaseChange : <xsl:value-of select="@TRUnitBaseChange"/>
          </div>
          <div class="ui-block-a">
            Unit BasePercentChange : <xsl:value-of select="@TRUnitBasePercentChange"/>
          </div>
          <div class="ui-block-b">    
          </div>
        </div>
      </div>
		</xsl:if>
    <div data-role="collapsible" data-collapsed="true" data-theme="b" data-content-theme="d" >
      <h4 class="ui-bar-b">
        <strong>SubBenefits</strong>
      </h4>
      <div class="ui-grid-a">
        <xsl:if test="(@TSubP2Name1 != '' and @TSubP2Name1 != 'none')">
          <div class="ui-block-a">
            SubBenefit 1 Name :  <xsl:value-of select="@TSubP2Name1"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 1 Amount : <xsl:value-of select="@TSubP2Amount1"/>
          </div>
          <div class="ui-block-a">
            SubBenefit 1 Unit :  <xsl:value-of select="@TSubP2Unit1"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 1 Price : <xsl:value-of select="@TSubP2Price1"/>
          </div>
          <div class="ui-block-a">
            SubBenefit 1 Total : <xsl:value-of select="@TSubP2Total1"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 1 Unit Benefit : <xsl:value-of select="@TSubP2TotalPerUnit1"/>
          </div>
          <div class="ui-block-a">
            SubBenefit 1 Description :  <xsl:value-of select="@TSubP2Description1"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 1 Label :  <xsl:value-of select="@TSubP2Label1"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Name2 != '' and @TSubP2Name2 != 'none')">
          <div class="ui-block-a">
            SubBenefit 2 Name :  <xsl:value-of select="@TSubP2Name2"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 2 Amount : <xsl:value-of select="@TSubP2Amount2"/>
          </div>
          <div class="ui-block-a">
            SubBenefit 2 Unit :  <xsl:value-of select="@TSubP2Unit2"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 2 Price : <xsl:value-of select="@TSubP2Price2"/>
          </div>
          <div class="ui-block-a">
            SubBenefit 2 Total : <xsl:value-of select="@TSubP2Total2"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 2 Unit Benefit : <xsl:value-of select="@TSubP2TotalPerUnit2"/>
          </div>
          <div class="ui-block-a">
            SubBenefit 2 Description :  <xsl:value-of select="@TSubP2Description2"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 2 Label :  <xsl:value-of select="@TSubP2Label2"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Name3 != '' and @TSubP2Name3 != 'none')">
          <div class="ui-block-a">
            SubBenefit 3 Name :  <xsl:value-of select="@TSubP2Name3"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 3 Amount : <xsl:value-of select="@TSubP2Amount3"/>
          </div>
          <div class="ui-block-a">
            SubBenefit 3 Unit :  <xsl:value-of select="@TSubP2Unit3"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 3 Price : <xsl:value-of select="@TSubP2Price3"/>
          </div>
          <div class="ui-block-a">
            SubBenefit 3 Total : <xsl:value-of select="@TSubP2Total3"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 3 Unit Benefit : <xsl:value-of select="@TSubP2TotalPerUnit3"/>
          </div>
          <div class="ui-block-a">
            SubBenefit 3 Description :  <xsl:value-of select="@TSubP2Description3"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 3 Label :  <xsl:value-of select="@TSubP2Label3"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Name4 != '' and @TSubP2Name4 != 'none')">
          <div class="ui-block-a">
            SubBenefit 4 Name :  <xsl:value-of select="@TSubP2Name4"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 4 Amount : <xsl:value-of select="@TSubP2Amount4"/>
          </div>
          <div class="ui-block-a">
            SubBenefit 4 Unit :  <xsl:value-of select="@TSubP2Unit4"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 4 Price : <xsl:value-of select="@TSubP2Price4"/>
          </div>
          <div class="ui-block-a">
            SubBenefit 4 Total : <xsl:value-of select="@TSubP2Total4"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 4 Unit Benefit : <xsl:value-of select="@TSubP2TotalPerUnit4"/>
          </div>
          <div class="ui-block-a">
            SubBenefit 4 Description :  <xsl:value-of select="@TSubP2Description4"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 4 Label :  <xsl:value-of select="@TSubP2Label4"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Name5 != '' and @TSubP2Name5 != 'none')">
          <div class="ui-block-a">
            SubBenefit 5 Name :  <xsl:value-of select="@TSubP2Name5"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 5 Amount : <xsl:value-of select="@TSubP2Amount5"/>
          </div>
          <div class="ui-block-a">
            SubBenefit 5 Unit :  <xsl:value-of select="@TSubP2Unit5"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 5 Price : <xsl:value-of select="@TSubP2Price5"/>
          </div>
          <div class="ui-block-a">
            SubBenefit 5 Total : <xsl:value-of select="@TSubP2Total5"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 5 Unit Benefit : <xsl:value-of select="@TSubP2TotalPerUnit5"/>
          </div>
          <div class="ui-block-a">
            SubBenefit 5 Description :  <xsl:value-of select="@TSubP2Description5"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 5 Label :  <xsl:value-of select="@TSubP2Label5"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Name6 != '' and @TSubP2Name6 != 'none')">
          <div class="ui-block-a">
            SubBenefit 6 Name :  <xsl:value-of select="@TSubP2Name6"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 6 Amount : <xsl:value-of select="@TSubP2Amount6"/>
          </div>
          <div class="ui-block-a">
            SubBenefit 6 Unit :  <xsl:value-of select="@TSubP2Unit6"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 6 Price : <xsl:value-of select="@TSubP2Price6"/>
          </div>
          <div class="ui-block-a">
            SubBenefit 6 Total : <xsl:value-of select="@TSubP2Total6"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 6 Unit Benefit : <xsl:value-of select="@TSubP2TotalPerUnit6"/>
          </div>
          <div class="ui-block-a">
            SubBenefit 6 Description :  <xsl:value-of select="@TSubP2Description6"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 6 Label :  <xsl:value-of select="@TSubP2Label6"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Name7 != '' and @TSubP2Name7 != 'none')">
          <div class="ui-block-a">
            SubBenefit 7 Name :  <xsl:value-of select="@TSubP2Name7"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 7 Amount : <xsl:value-of select="@TSubP2Amount7"/>
          </div>
          <div class="ui-block-a">
            SubBenefit 7 Unit :  <xsl:value-of select="@TSubP2Unit7"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 7 Price : <xsl:value-of select="@TSubP2Price7"/>
          </div>
          <div class="ui-block-a">
            SubBenefit 7 Total : <xsl:value-of select="@TSubP2Total7"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 7 Unit Benefit : <xsl:value-of select="@TSubP2TotalPerUnit7"/>
          </div>
          <div class="ui-block-a">
            SubBenefit 7 Description :  <xsl:value-of select="@TSubP2Description7"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 7 Label :  <xsl:value-of select="@TSubP2Label7"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Name8 != '' and @TSubP2Name8 != 'none')">
          <div class="ui-block-a">
            SubBenefit 8 Name :  <xsl:value-of select="@TSubP2Name8"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 8 Amount : <xsl:value-of select="@TSubP2Amount8"/>
          </div>
          <div class="ui-block-a">
            SubBenefit 8 Unit :  <xsl:value-of select="@TSubP2Unit8"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 8 Price : <xsl:value-of select="@TSubP2Price8"/>
          </div>
          <div class="ui-block-a">
            SubBenefit 8 Total : <xsl:value-of select="@TSubP2Total8"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 8 Unit Benefit : <xsl:value-of select="@TSubP2TotalPerUnit8"/>
          </div>
          <div class="ui-block-a">
            SubBenefit 8 Description :  <xsl:value-of select="@TSubP2Description8"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 8 Label :  <xsl:value-of select="@TSubP2Label8"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Name9 != '' and @TSubP2Name9 != 'none')">
          <div class="ui-block-a">
            SubBenefit 9 Name :  <xsl:value-of select="@TSubP2Name9"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 9 Amount : <xsl:value-of select="@TSubP2Amount9"/>
          </div>
          <div class="ui-block-a">
            SubBenefit 9 Unit :  <xsl:value-of select="@TSubP2Unit9"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 9 Price : <xsl:value-of select="@TSubP2Price9"/>
          </div>
          <div class="ui-block-a">
            SubBenefit 9 Total : <xsl:value-of select="@TSubP2Total9"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 9 Unit Benefit : <xsl:value-of select="@TSubP2TotalPerUnit9"/>
          </div>
          <div class="ui-block-a">
            SubBenefit 9 Description :  <xsl:value-of select="@TSubP2Description9"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 9 Label :  <xsl:value-of select="@TSubP2Label9"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Name10 != '' and @TSubP2Name10 != 'none')">
          <div class="ui-block-a">
            SubBenefit 10 Name :  <xsl:value-of select="@TSubP2Name10"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 10 Amount : <xsl:value-of select="@TSubP2Amount10"/>
          </div>
          <div class="ui-block-a">
            SubBenefit 10 Unit :  <xsl:value-of select="@TSubP2Unit10"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 10 Price : <xsl:value-of select="@TSubP2Price10"/>
          </div>
          <div class="ui-block-a">
            SubBenefit 10 Total : <xsl:value-of select="@TSubP2Total10"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 10 Unit Benefit : <xsl:value-of select="@TSubP2TotalPerUnit10"/>
          </div>
          <div class="ui-block-a">
            SubBenefit 10 Description :  <xsl:value-of select="@TSubP2Description10"/>
          </div>
          <div class="ui-block-b">
            SubBenefit 10 Label :  <xsl:value-of select="@TSubP2Label10"/>
          </div>
        </xsl:if>
      </div>
    </div>
	</xsl:template>
</xsl:stylesheet>
