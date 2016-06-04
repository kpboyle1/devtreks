<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2014, Oct -->
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
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
    <xsl:choose>
      <xsl:when test="(root/linkedview[@Id=$calculatorid])">
        <xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			    <xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		    </xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcatotal1']">
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
      <strong>Input</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:variable name="calculatorid"><xsl:value-of select="@CalculatorId"/></xsl:variable>
    <xsl:choose>
      <xsl:when test="(root/linkedview[@Id=$calculatorid])">
        <xsl:apply-templates select="root/linkedview[@Id=$calculatorid]">
			    <xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		    </xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcatotal1']">
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
				<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcatotal1']">
			    <xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		    </xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
		<xsl:if test="(@TSubP1Name1 != '')">
      <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Cost Details</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
           Total OC : <xsl:value-of select="@TOCCost"/>
          </div>
          <div class="ui-block-b">
           Total AOH : <xsl:value-of select="@TAOHCost"/>
          </div>
          <div class="ui-block-a">
           Total CAP : <xsl:value-of select="@TCAPCost"/>
          </div>
          <div class="ui-block-b">
           Total LCC : <xsl:value-of select="@TLCCCost"/>
          </div>
          <div class="ui-block-a">
            Total EAA : <xsl:value-of select="@TEAACost"/>
          </div>
          <div class="ui-block-b">
            Total Unit : <xsl:value-of select="@TUnitCost"/>
          </div>
          <xsl:if test="(@TSubP1Name1 != '' and @TSubP1Name1 != 'none' )">
            <div class="ui-block-a">
              SubCost 1 Name :  <xsl:value-of select="@TSubP1Name1"/>
            </div>
            <div class="ui-block-b">
              SubCost 1 Amount : <xsl:value-of select="@TSubP1Amount1"/>
            </div>
            <div class="ui-block-a">
              SubCost 1 Unit : <xsl:value-of select="@TSubP1Unit1"/>
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
          <xsl:if test="(@TSubP1Name2 != ''  and @TSubP1Name2 != 'none' )">
            <div class="ui-block-a">
              SubCost 2 Name :  <xsl:value-of select="@TSubP1Name2"/>
            </div>
            <div class="ui-block-b">
              SubCost 2 Amount : <xsl:value-of select="@TSubP1Amount2"/>
            </div>
            <div class="ui-block-a">
             SubCost 2 Unit : <xsl:value-of select="@TSubP1Unit2"/>
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
          <xsl:if test="(@TSubP1Name3 != '' and @TSubP1Name3 != 'none' )">
            <div class="ui-block-a">
              SubCost 3 Name :  <xsl:value-of select="@TSubP1Name3"/>
            </div>
            <div class="ui-block-b">
              SubCost 3 Amount : <xsl:value-of select="@TSubP1Amount3"/>
            </div>
            <div class="ui-block-a">
              SubCost 3 Unit : <xsl:value-of select="@TSubP1Unit3"/>
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
          <xsl:if test="(@TSubP1Name4 != '' and @TSubP1Name4 != 'none' )">
            <div class="ui-block-a">
              SubCost 4 Name :  <xsl:value-of select="@TSubP1Name4"/>
            </div>
            <div class="ui-block-b">
              SubCost 4 Amount : <xsl:value-of select="@TSubP1Amount4"/>
            </div>
            <div class="ui-block-a">
             SubCost 4 Unit : <xsl:value-of select="@TSubP1Unit4"/>
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
          <xsl:if test="(@TSubP1Name5 != '' and @TSubP1Name5 != 'none' )">
            <div class="ui-block-a">
              SubCost 5 Name :  <xsl:value-of select="@TSubP1Name5"/>
            </div>
            <div class="ui-block-b">
              SubCost 5 Amount : <xsl:value-of select="@TSubP1Amount5"/>
            </div>
            <div class="ui-block-a">
              SubCost 5 Unit : <xsl:value-of select="@TSubP1Unit5"/>
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
          <xsl:if test="(@TSubP1Name6 != '' and @TSubP1Name6 != 'none' )">
            <div class="ui-block-a">
              SubCost 6 Name :  <xsl:value-of select="@TSubP1Name6"/>
            </div>
            <div class="ui-block-b">
              SubCost 6 Amount : <xsl:value-of select="@TSubP1Amount6"/>
            </div>
            <div class="ui-block-a">
             SubCost 6 Unit : <xsl:value-of select="@TSubP1Unit6"/>
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
          <xsl:if test="(@TSubP1Name7 != '' and @TSubP1Name7 != 'none' )">
            <div class="ui-block-a">
              SubCost 7 Name :  <xsl:value-of select="@TSubP1Name7"/>
            </div>
            <div class="ui-block-b">
              SubCost 7 Amount : <xsl:value-of select="@TSubP1Amount7"/>
            </div>
            <div class="ui-block-a">
              SubCost 7 Unit : <xsl:value-of select="@TSubP1Unit7"/>
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
          <xsl:if test="(@TSubP1Name8 != '' and @TSubP1Name8 != 'none' )">
            <div class="ui-block-a">
              SubCost 8 Name :  <xsl:value-of select="@TSubP1Name8"/>
            </div>
            <div class="ui-block-b">
              SubCost 8 Amount : <xsl:value-of select="@TSubP1Amount8"/>
            </div>
            <div class="ui-block-a">
              SubCost 8 Unit : <xsl:value-of select="@TSubP1Unit8"/>
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
          <xsl:if test="(@TSubP1Name9 != '' and @TSubP1Name9 != 'none' )">
            <div class="ui-block-a">
              SubCost 9 Name :  <xsl:value-of select="@TSubP1Name9"/>
            </div>
            <div class="ui-block-b">
              SubCost 9 Amount : <xsl:value-of select="@TSubP1Amount9"/>
            </div>
            <div class="ui-block-a">
              SubCost 9 Unit : <xsl:value-of select="@TSubP1Unit9"/>
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
          <xsl:if test="(@TSubP1Name10 != '' and @TSubP1Name10 != 'none' )">
            <div class="ui-block-a">
              SubCost 10 Name :  <xsl:value-of select="@TSubP1Name10"/>
            </div>
            <div class="ui-block-b">
              SubCost 10 Amount : <xsl:value-of select="@TSubP1Amount10"/>
            </div>
            <div class="ui-block-a">
              SubCost 10 Unit : <xsl:value-of select="@TSubP1Unit10"/>
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
		</xsl:if>
		<xsl:if test="(@SubPName1 != '')">
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Cost Details</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
           Total OC : <xsl:value-of select="@OCTotalCost"/>
          </div>
          <div class="ui-block-b">
            Total AOH : <xsl:value-of select="@AOHTotalCost"/>
          </div>
          <div class="ui-block-a">
            Total CAP : <xsl:value-of select="@CAPTotalCost"/>
          </div>
          <div class="ui-block-b">
           Total LCC : <xsl:value-of select="@LCCTotalCost"/>
          </div>
          <div class="ui-block-a">
            Total EAA : <xsl:value-of select="@EAATotalCost"/>
          </div>
          <div class="ui-block-b">
            
          </div>
          <xsl:if test="(@SubPName1 != '' and @SubPName1 != 'none')">
            <div class="ui-block-a">
              SubCost 1 Name :  <xsl:value-of select="@SubPName1"/>
            </div>
            <div class="ui-block-b">
              SubCost 1 Amount : <xsl:value-of select="@SubPAmount1"/>
            </div>
            <div class="ui-block-a">
              SubCost 1 Unit :  <xsl:value-of select="@SubPUnit1"/>
            </div>
            <div class="ui-block-b">
              SubCost 1 Price : <xsl:value-of select="@SubPPrice1"/>
            </div>
            <div class="ui-block-a">
              SubCost 1 Total : <xsl:value-of select="@SubPTotal1"/>
            </div>
            <div class="ui-block-b">
              SubCost 1 Unit Cost : <xsl:value-of select="@SubPTotalPerUnit1"/>
            </div>
            <div class="ui-block-a">
              SubCost 1 Description :  <xsl:value-of select="@SubPDescription1"/>
            </div>
            <div class="ui-block-b">
              SubCost 1 Label :  <xsl:value-of select="@SubPLabel1"/>
            </div>
          </xsl:if>
          <xsl:if test="(@SubPName2 != '' and @SubPName2 != 'none')">
            <div class="ui-block-a">
              SubCost 2 Name :  <xsl:value-of select="@SubPName2"/>
            </div>
            <div class="ui-block-b">
              SubCost 2 Amount : <xsl:value-of select="@SubPAmount2"/>
            </div>
            <div class="ui-block-a">
              SubCost 2 Unit :  <xsl:value-of select="@SubPUnit2"/>
            </div>
            <div class="ui-block-b">
              SubCost 2 Price : <xsl:value-of select="@SubPPrice2"/>
            </div>
            <div class="ui-block-a">
              SubCost 2 Total : <xsl:value-of select="@SubPTotal2"/>
            </div>
            <div class="ui-block-b">
              SubCost 2 Unit Cost : <xsl:value-of select="@SubPTotalPerUnit2"/>
            </div>
            <div class="ui-block-a">
              SubCost 2 Description :  <xsl:value-of select="@SubPDescription2"/>
            </div>
            <div class="ui-block-b">
              SubCost 2 Label :  <xsl:value-of select="@SubPLabel2"/>
            </div>
          </xsl:if>
          <xsl:if test="(@SubPName3 != '' and @SubPName3 != 'none')">
            <div class="ui-block-a">
              SubCost 3 Name :  <xsl:value-of select="@SubPName3"/>
            </div>
            <div class="ui-block-b">
              SubCost 3 Amount : <xsl:value-of select="@SubPAmount3"/>
            </div>
            <div class="ui-block-a">
              SubCost 3 Unit :  <xsl:value-of select="@SubPUnit3"/>
            </div>
            <div class="ui-block-b">
              SubCost 3 Price : <xsl:value-of select="@SubPPrice3"/>
            </div>
            <div class="ui-block-a">
              SubCost 3 Total : <xsl:value-of select="@SubPTotal3"/>
            </div>
            <div class="ui-block-b">
              SubCost 3 Unit Cost : <xsl:value-of select="@SubPTotalPerUnit3"/>
            </div>
            <div class="ui-block-a">
              SubCost 3 Description :  <xsl:value-of select="@SubPDescription3"/>
            </div>
            <div class="ui-block-b">
              SubCost 3 Label :  <xsl:value-of select="@SubPLabel3"/>
            </div>
          </xsl:if>
          <xsl:if test="(@SubPName4 != '' and @SubPName4 != 'none')">
            <div class="ui-block-a">
              SubCost 4 Name :  <xsl:value-of select="@SubPName4"/>
            </div>
            <div class="ui-block-b">
              SubCost 4 Amount : <xsl:value-of select="@SubPAmount4"/>
            </div>
            <div class="ui-block-a">
              SubCost 4 Unit :  <xsl:value-of select="@SubPUnit4"/>
            </div>
            <div class="ui-block-b">
              SubCost 4 Price : <xsl:value-of select="@SubPPrice4"/>
            </div>
            <div class="ui-block-a">
              SubCost 4 Total : <xsl:value-of select="@SubPTotal4"/>
            </div>
            <div class="ui-block-b">
              SubCost 4 Unit Cost : <xsl:value-of select="@SubPTotalPerUnit4"/>
            </div>
            <div class="ui-block-a">
              SubCost 4 Description :  <xsl:value-of select="@SubPDescription4"/>
            </div>
            <div class="ui-block-b">
              SubCost 4 Label :  <xsl:value-of select="@SubPLabel4"/>
            </div>
          </xsl:if>
          <xsl:if test="(@SubPName5 != '' and @SubPName5 != 'none')">
            <div class="ui-block-a">
              SubCost 5 Name :  <xsl:value-of select="@SubPName5"/>
            </div>
            <div class="ui-block-b">
              SubCost 5 Amount : <xsl:value-of select="@SubPAmount5"/>
            </div>
            <div class="ui-block-a">
              SubCost 5 Unit :  <xsl:value-of select="@SubPUnit5"/>
            </div>
            <div class="ui-block-b">
              SubCost 5 Price : <xsl:value-of select="@SubPPrice5"/>
            </div>
            <div class="ui-block-a">
              SubCost 5 Total : <xsl:value-of select="@SubPTotal5"/>
            </div>
            <div class="ui-block-b">
              SubCost 5 Unit Cost : <xsl:value-of select="@SubPTotalPerUnit5"/>
            </div>
            <div class="ui-block-a">
              SubCost 5 Description :  <xsl:value-of select="@SubPDescription5"/>
            </div>
            <div class="ui-block-b">
              SubCost 5 Label :  <xsl:value-of select="@SubPLabel5"/>
            </div>
          </xsl:if>
          <xsl:if test="(@SubPName6 != '' and @SubPName6 != 'none')">
            <div class="ui-block-a">
              SubCost 6 Name :  <xsl:value-of select="@SubPName6"/>
            </div>
            <div class="ui-block-b">
              SubCost 6 Amount : <xsl:value-of select="@SubPAmount6"/>
            </div>
            <div class="ui-block-a">
              SubCost 6 Unit :  <xsl:value-of select="@SubPUnit6"/>
            </div>
            <div class="ui-block-b">
              SubCost 6 Price : <xsl:value-of select="@SubPPrice6"/>
            </div>
            <div class="ui-block-a">
              SubCost 6 Total : <xsl:value-of select="@SubPTotal6"/>
            </div>
            <div class="ui-block-b">
              SubCost 6 Unit Cost : <xsl:value-of select="@SubPTotalPerUnit6"/>
            </div>
            <div class="ui-block-a">
              SubCost 6 Description :  <xsl:value-of select="@SubPDescription6"/>
            </div>
            <div class="ui-block-b">
              SubCost 6 Label :  <xsl:value-of select="@SubPLabel6"/>
            </div>
          </xsl:if>
          <xsl:if test="(@SubPName7 != '' and @SubPName7 != 'none')">
            <div class="ui-block-a">
              SubCost 7 Name :  <xsl:value-of select="@SubPName7"/>
            </div>
            <div class="ui-block-b">
              SubCost 7 Amount : <xsl:value-of select="@SubPAmount7"/>
            </div>
            <div class="ui-block-a">
              SubCost 7 Unit :  <xsl:value-of select="@SubPUnit7"/>
            </div>
            <div class="ui-block-b">
              SubCost 7 Price : <xsl:value-of select="@SubPPrice7"/>
            </div>
            <div class="ui-block-a">
              SubCost 7 Total : <xsl:value-of select="@SubPTotal7"/>
            </div>
            <div class="ui-block-b">
              SubCost 7 Unit Cost : <xsl:value-of select="@SubPTotalPerUnit7"/>
            </div>
            <div class="ui-block-a">
              SubCost 7 Description :  <xsl:value-of select="@SubPDescription7"/>
            </div>
            <div class="ui-block-b">
              SubCost 7 Label :  <xsl:value-of select="@SubPLabel7"/>
            </div>
          </xsl:if>
          <xsl:if test="(@SubPName8 != '' and @SubPName8 != 'none')">
            <div class="ui-block-a">
              SubCost 8 Name :  <xsl:value-of select="@SubPName8"/>
            </div>
            <div class="ui-block-b">
              SubCost 8 Amount : <xsl:value-of select="@SubPAmount8"/>
            </div>
            <div class="ui-block-a">
              SubCost 8 Unit :  <xsl:value-of select="@SubPUnit8"/>
            </div>
            <div class="ui-block-b">
              SubCost 8 Price : <xsl:value-of select="@SubPPrice8"/>
            </div>
            <div class="ui-block-a">
              SubCost 8 Total : <xsl:value-of select="@SubPTotal8"/>
            </div>
            <div class="ui-block-b">
              SubCost 8 Unit Cost : <xsl:value-of select="@SubPTotalPerUnit8"/>
            </div>
            <div class="ui-block-a">
              SubCost 8 Description :  <xsl:value-of select="@SubPDescription8"/>
            </div>
            <div class="ui-block-b">
              SubCost 8 Label :  <xsl:value-of select="@SubPLabel8"/>
            </div>
          </xsl:if>
          <xsl:if test="(@SubPName9 != '' and @SubPName9 != 'none')">
            <div class="ui-block-a">
              SubCost 9 Name :  <xsl:value-of select="@SubPName9"/>
            </div>
            <div class="ui-block-b">
              SubCost 9 Amount : <xsl:value-of select="@SubPAmount9"/>
            </div>
            <div class="ui-block-a">
              SubCost 9 Unit :  <xsl:value-of select="@SubPUnit9"/>
            </div>
            <div class="ui-block-b">
              SubCost 9 Price : <xsl:value-of select="@SubPPrice9"/>
            </div>
            <div class="ui-block-a">
              SubCost 9 Total : <xsl:value-of select="@SubPTotal9"/>
            </div>
            <div class="ui-block-b">
              SubCost 9 Unit Cost : <xsl:value-of select="@SubPTotalPerUnit9"/>
            </div>
            <div class="ui-block-a">
              SubCost 9 Description :  <xsl:value-of select="@SubPDescription9"/>
            </div>
            <div class="ui-block-b">
              SubCost 9 Label :  <xsl:value-of select="@SubPLabel9"/>
            </div>
          </xsl:if>
          <xsl:if test="(@SubPName10 != '' and @SubPName10 != 'none')">
            <div class="ui-block-a">
              SubCost 10 Name :  <xsl:value-of select="@SubPName10"/>
            </div>
            <div class="ui-block-b">
              SubCost 10 Amount : <xsl:value-of select="@SubPAmount10"/>
            </div>
            <div class="ui-block-a">
              SubCost 10 Unit :  <xsl:value-of select="@SubPUnit10"/>
            </div>
            <div class="ui-block-b">
              SubCost 10 Price : <xsl:value-of select="@SubPPrice10"/>
            </div>
            <div class="ui-block-a">
              SubCost 10 Total : <xsl:value-of select="@SubPTotal10"/>
            </div>
            <div class="ui-block-b">
              SubCost 10 Unit Cost : <xsl:value-of select="@SubPTotalPerUnit10"/>
            </div>
            <div class="ui-block-a">
              SubCost 10 Description :  <xsl:value-of select="@SubPDescription10"/>
            </div>
            <div class="ui-block-b">
              SubCost 10 Label :  <xsl:value-of select="@SubPLabel10"/>
            </div>
          </xsl:if>
          <div class="ui-block-a">
             Unit Amount : <xsl:value-of select="@PerUnitAmount"/>
          </div>
          <div class="ui-block-b">
           Unit : <xsl:value-of select="@PerUnitUnit"/>
          </div>
          <div class="ui-block-a">
            Total Unit Cost : <xsl:value-of select="@UnitTotalCost"/>
          </div>
          <div class="ui-block-b">
           Service Life : <xsl:value-of select="@ServiceLifeYears"/>
          </div>
          <div class="ui-block-a">
             P/C Years : <xsl:value-of select="@PlanningConstructionYears"/>
          </div>
          <div class="ui-block-b">
           Yrs From Base Date : <xsl:value-of select="@YearsFromBaseDate"/>
          </div>
          <div class="ui-block-a">
            Target Type : <xsl:value-of select="@TargetType"/>
          </div>
          <div class="ui-block-b">
            Altern Type  : <xsl:value-of select="@AlternativeType"/>
          </div>
        </div>
         <div >
			    <strong>Description : </strong><xsl:value-of select="@CalculatorDescription" />
	      </div>
      </div>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
