<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2014, Jan -->
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
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='meprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="output">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="output">
		<h4>
      <strong>Output </strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='meprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="outputseries">
			<xsl:sort select="@OutputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outputseries">
		<h4>
      <strong>Output Series </strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='meprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
	  <xsl:param name="localName" />
    <xsl:if test="(@TargetType != '' and @TargetType != 'none')">
      <div>
			  Target Type: <strong><xsl:value-of select="@TargetType"/></strong>
	    </div>
    </xsl:if>
    <xsl:if test="(@TME2Name1 != '')">
      <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
      <h4 class="ui-bar-b">
        <strong>Indicator Details</strong>
      </h4>
      <div class="ui-grid-a">
        <div class="ui-block-a">
          Indicator 1 Name : <strong><xsl:value-of select="@TME2Name1"/></strong>
        </div>
        <div class="ui-block-b">
          Label : <xsl:value-of select="@TME2Label1"/>
        </div>
        <div class="ui-block-a">
          Date : <xsl:value-of select="@TME2Date1"/>
        </div>
        <div class="ui-block-b">
          Observations : <xsl:value-of select="@TME2N1"/>
        </div>
        <div class="ui-block-a">
          Unit : <xsl:value-of select="@TME2Unit1"/>
        </div>
        <div class="ui-block-b">
           Type : <xsl:value-of select="@TME2Type1"/>
        </div>
        <div class="ui-block-a">
          Total Planned Period : <xsl:value-of select="@TME2Total1"/>
        </div>
        <div class="ui-block-b">
          Total Plan Full : <xsl:value-of select="@TPFTotal1"/>
        </div>
        <div class="ui-block-a">
          Total Plan Cumul : <xsl:value-of select="@TPCTotal1"/>
        </div>
        <div class="ui-block-b">
          Total Actual Period : <xsl:value-of select="@TAPTotal1"/>
        </div>
        <div class="ui-block-a">
          Total Actual Cumul : <xsl:value-of select="@TACTotal1"/>
        </div>
        <div class="ui-block-b">
          Total Actual Period Change : <xsl:value-of select="@TAPChange1"/>
        </div>
        <div class="ui-block-a">
          Total Actual Cumul Change : <xsl:value-of select="@TACChange1"/>
        </div>
        <div class="ui-block-b">
          Total Planned Period Percent : <xsl:value-of select="@TPPPercent1"/>
        </div>
        <div class="ui-block-a">
          Total Planned Cumul Percent : <xsl:value-of select="@TPCPercent1"/>
        </div>
        <div class="ui-block-b">
          Total Planned Full Percent : <xsl:value-of select="@TPFPercent1"/>
        </div>
        <div class="ui-block-a">
          Q1 Unit : <xsl:value-of select="@TME2Q1Unit1"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Q1 Planned Period : <xsl:value-of select="@TME2Q1Total1"/>
        </div>
        <div class="ui-block-b">
          Q1 Plan Full : <xsl:value-of select="@TQ1PFTotal1"/>
        </div>
        <div class="ui-block-a">
          Q1 Plan Cumul : <xsl:value-of select="@TQ1PCTotal1"/>
        </div>
        <div class="ui-block-b">
          Q1 Actual Period : <xsl:value-of select="@TQ1APTotal1"/>
        </div>
        <div class="ui-block-a">
          Q1 Actual Cumul : <xsl:value-of select="@TQ1ACTotal1"/>
        </div>
        <div class="ui-block-b">
          Q1 Actual Period Change : <xsl:value-of select="@TQ1APChange1"/>
        </div>
        <div class="ui-block-a">
          Q1 Actual Cumul Change : <xsl:value-of select="@TQ1ACChange1"/>
        </div>
        <div class="ui-block-b">
          Q1 Planned Period Percent : <xsl:value-of select="@TQ1PPPercent1"/>
        </div>
        <div class="ui-block-a">
          Q1 Planned Cumul Percent : <xsl:value-of select="@TQ1PCPercent1"/>
        </div>
        <div class="ui-block-b">
          Q1 Planned Full Percent : <xsl:value-of select="@TQ1PFPercent1"/>
        </div>
        <div class="ui-block-a">
          Q2 Unit : <xsl:value-of select="@TME2Q2Unit1"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Q2 Planned Period : <xsl:value-of select="@TME2Q2Total1"/>
        </div>
        <div class="ui-block-b">
          Q2 Plan Full : <xsl:value-of select="@TQ2PFTotal1"/>
        </div>
        <div class="ui-block-a">
          Q2 Plan Cumul : <xsl:value-of select="@TQ2PCTotal1"/>
        </div>
        <div class="ui-block-b">
          Q2 Actual Period : <xsl:value-of select="@TQ2APTotal1"/>
        </div>
        <div class="ui-block-a">
          Q2 Actual Cumul : <xsl:value-of select="@TQ2ACTotal1"/>
        </div>
        <div class="ui-block-b">
          Q2 Actual Period Change : <xsl:value-of select="@TQ2APChange1"/>
        </div>
        <div class="ui-block-a">
          Q2 Actual Cumul Change : <xsl:value-of select="@TQ2ACChange1"/>
        </div>
        <div class="ui-block-b">
          Q2 Planned Period Percent : <xsl:value-of select="@TQ2PPPercent1"/>
        </div>
        <div class="ui-block-a">
          Q2 Planned Cumul Percent : <xsl:value-of select="@TQ2PCPercent1"/>
        </div>
        <div class="ui-block-b">
          Q2 Planned Full Percent : <xsl:value-of select="@TQ2PFPercent1"/>
        </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description1" />
	      </div>
      </div>
	</xsl:if>
  <xsl:if test="(@TME2Name2 != '')">
      <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
      <h4 class="ui-bar-b">
        <strong>Indicator Details</strong>
      </h4>
      <div class="ui-grid-a">
        <div class="ui-block-a">
          Indicator 2 Name : <strong><xsl:value-of select="@TME2Name2"/></strong>
        </div>
        <div class="ui-block-b">
          Label : <xsl:value-of select="@TME2Label2"/>
        </div>
        <div class="ui-block-a">
          Date : <xsl:value-of select="@TME2Date2"/>
        </div>
        <div class="ui-block-b">
          Observations : <xsl:value-of select="@TME2N2"/>
        </div>
        <div class="ui-block-a">
          Unit : <xsl:value-of select="@TME2Unit2"/>
        </div>
        <div class="ui-block-b">
           Type : <xsl:value-of select="@TME2Type2"/>
        </div>
        <div class="ui-block-a">
          Total Planned Period : <xsl:value-of select="@TME2Total2"/>
        </div>
        <div class="ui-block-b">
          Total Plan Full : <xsl:value-of select="@TPFTotal2"/>
        </div>
        <div class="ui-block-a">
          Total Plan Cumul : <xsl:value-of select="@TPCTotal2"/>
        </div>
        <div class="ui-block-b">
          Total Actual Period : <xsl:value-of select="@TAPTotal2"/>
        </div>
        <div class="ui-block-a">
          Total Actual Cumul : <xsl:value-of select="@TACTotal2"/>
        </div>
        <div class="ui-block-b">
          Total Actual Period Change : <xsl:value-of select="@TAPChange2"/>
        </div>
        <div class="ui-block-a">
          Total Actual Cumul Change : <xsl:value-of select="@TACChange2"/>
        </div>
        <div class="ui-block-b">
          Total Planned Period Percent : <xsl:value-of select="@TPPPercent2"/>
        </div>
        <div class="ui-block-a">
          Total Planned Cumul Percent : <xsl:value-of select="@TPCPercent2"/>
        </div>
        <div class="ui-block-b">
          Total Planned Full Percent : <xsl:value-of select="@TPFPercent2"/>
        </div>
        <div class="ui-block-a">
          Q1 Unit : <xsl:value-of select="@TME2Q1Unit2"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Q1 Planned Period : <xsl:value-of select="@TME2Q1Total2"/>
        </div>
        <div class="ui-block-b">
          Q1 Plan Full : <xsl:value-of select="@TQ1PFTotal2"/>
        </div>
        <div class="ui-block-a">
          Q1 Plan Cumul : <xsl:value-of select="@TQ1PCTotal2"/>
        </div>
        <div class="ui-block-b">
          Q1 Actual Period : <xsl:value-of select="@TQ1APTotal2"/>
        </div>
        <div class="ui-block-a">
          Q1 Actual Cumul : <xsl:value-of select="@TQ1ACTotal2"/>
        </div>
        <div class="ui-block-b">
          Q1 Actual Period Change : <xsl:value-of select="@TQ1APChange2"/>
        </div>
        <div class="ui-block-a">
          Q1 Actual Cumul Change : <xsl:value-of select="@TQ1ACChange2"/>
        </div>
        <div class="ui-block-b">
          Q1 Planned Period Percent : <xsl:value-of select="@TQ1PPPercent2"/>
        </div>
        <div class="ui-block-a">
          Q1 Planned Cumul Percent : <xsl:value-of select="@TQ1PCPercent2"/>
        </div>
        <div class="ui-block-b">
          Q1 Planned Full Percent : <xsl:value-of select="@TQ1PFPercent2"/>
        </div>
        <div class="ui-block-a">
          Q2 Unit : <xsl:value-of select="@TME2Q2Unit2"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Q2 Planned Period : <xsl:value-of select="@TME2Q2Total2"/>
        </div>
        <div class="ui-block-b">
          Q2 Plan Full : <xsl:value-of select="@TQ2PFTotal2"/>
        </div>
        <div class="ui-block-a">
          Q2 Plan Cumul : <xsl:value-of select="@TQ2PCTotal2"/>
        </div>
        <div class="ui-block-b">
          Q2 Actual Period : <xsl:value-of select="@TQ2APTotal2"/>
        </div>
        <div class="ui-block-a">
          Q2 Actual Cumul : <xsl:value-of select="@TQ2ACTotal2"/>
        </div>
        <div class="ui-block-b">
          Q2 Actual Period Change : <xsl:value-of select="@TQ2APChange2"/>
        </div>
        <div class="ui-block-a">
          Q2 Actual Cumul Change : <xsl:value-of select="@TQ2ACChange2"/>
        </div>
        <div class="ui-block-b">
          Q2 Planned Period Percent : <xsl:value-of select="@TQ2PPPercent2"/>
        </div>
        <div class="ui-block-a">
          Q2 Planned Cumul Percent : <xsl:value-of select="@TQ2PCPercent2"/>
        </div>
        <div class="ui-block-b">
          Q2 Planned Full Percent : <xsl:value-of select="@TQ2PFPercent2"/>
        </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description2" />
	      </div>
      </div>
	</xsl:if>
  <xsl:if test="(@TME2Name3 != '')">
      <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
      <h4 class="ui-bar-b">
        <strong>Indicator Details</strong>
      </h4>
      <div class="ui-grid-a">
        <div class="ui-block-a">
          Indicator 3 Name : <strong><xsl:value-of select="@TME2Name3"/></strong>
        </div>
        <div class="ui-block-b">
          Label : <xsl:value-of select="@TME2Label3"/>
        </div>
        <div class="ui-block-a">
          Date : <xsl:value-of select="@TME2Date3"/>
        </div>
        <div class="ui-block-b">
          Observations : <xsl:value-of select="@TME2N3"/>
        </div>
        <div class="ui-block-a">
          Unit : <xsl:value-of select="@TME2Unit3"/>
        </div>
        <div class="ui-block-b">
           Type : <xsl:value-of select="@TME2Type3"/>
        </div>
        <div class="ui-block-a">
          Total Planned Period : <xsl:value-of select="@TME2Total3"/>
        </div>
        <div class="ui-block-b">
          Total Plan Full : <xsl:value-of select="@TPFTotal3"/>
        </div>
        <div class="ui-block-a">
          Total Plan Cumul : <xsl:value-of select="@TPCTotal3"/>
        </div>
        <div class="ui-block-b">
          Total Actual Period : <xsl:value-of select="@TAPTotal3"/>
        </div>
        <div class="ui-block-a">
          Total Actual Cumul : <xsl:value-of select="@TACTotal3"/>
        </div>
        <div class="ui-block-b">
          Total Actual Period Change : <xsl:value-of select="@TAPChange3"/>
        </div>
        <div class="ui-block-a">
          Total Actual Cumul Change : <xsl:value-of select="@TACChange3"/>
        </div>
        <div class="ui-block-b">
          Total Planned Period Percent : <xsl:value-of select="@TPPPercent3"/>
        </div>
        <div class="ui-block-a">
          Total Planned Cumul Percent : <xsl:value-of select="@TPCPercent3"/>
        </div>
        <div class="ui-block-b">
          Total Planned Full Percent : <xsl:value-of select="@TPFPercent3"/>
        </div>
        <div class="ui-block-a">
          Q1 Unit : <xsl:value-of select="@TME2Q1Unit3"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Q1 Planned Period : <xsl:value-of select="@TME2Q1Total3"/>
        </div>
        <div class="ui-block-b">
          Q1 Plan Full : <xsl:value-of select="@TQ1PFTotal3"/>
        </div>
        <div class="ui-block-a">
          Q1 Plan Cumul : <xsl:value-of select="@TQ1PCTotal3"/>
        </div>
        <div class="ui-block-b">
          Q1 Actual Period : <xsl:value-of select="@TQ1APTotal3"/>
        </div>
        <div class="ui-block-a">
          Q1 Actual Cumul : <xsl:value-of select="@TQ1ACTotal3"/>
        </div>
        <div class="ui-block-b">
          Q1 Actual Period Change : <xsl:value-of select="@TQ1APChange3"/>
        </div>
        <div class="ui-block-a">
          Q1 Actual Cumul Change : <xsl:value-of select="@TQ1ACChange3"/>
        </div>
        <div class="ui-block-b">
          Q1 Planned Period Percent : <xsl:value-of select="@TQ1PPPercent3"/>
        </div>
        <div class="ui-block-a">
          Q1 Planned Cumul Percent : <xsl:value-of select="@TQ1PCPercent3"/>
        </div>
        <div class="ui-block-b">
          Q1 Planned Full Percent : <xsl:value-of select="@TQ1PFPercent3"/>
        </div>
        <div class="ui-block-a">
          Q2 Unit : <xsl:value-of select="@TME2Q2Unit3"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Q2 Planned Period : <xsl:value-of select="@TME2Q2Total3"/>
        </div>
        <div class="ui-block-b">
          Q2 Plan Full : <xsl:value-of select="@TQ2PFTotal3"/>
        </div>
        <div class="ui-block-a">
          Q2 Plan Cumul : <xsl:value-of select="@TQ2PCTotal3"/>
        </div>
        <div class="ui-block-b">
          Q2 Actual Period : <xsl:value-of select="@TQ2APTotal3"/>
        </div>
        <div class="ui-block-a">
          Q2 Actual Cumul : <xsl:value-of select="@TQ2ACTotal3"/>
        </div>
        <div class="ui-block-b">
          Q2 Actual Period Change : <xsl:value-of select="@TQ2APChange3"/>
        </div>
        <div class="ui-block-a">
          Q2 Actual Cumul Change : <xsl:value-of select="@TQ2ACChange3"/>
        </div>
        <div class="ui-block-b">
          Q2 Planned Period Percent : <xsl:value-of select="@TQ2PPPercent3"/>
        </div>
        <div class="ui-block-a">
          Q2 Planned Cumul Percent : <xsl:value-of select="@TQ2PCPercent3"/>
        </div>
        <div class="ui-block-b">
          Q2 Planned Full Percent : <xsl:value-of select="@TQ2PFPercent3"/>
        </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description3" />
	      </div>
      </div>
	</xsl:if>
  <xsl:if test="(@TME2Name4 != '')">
      <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
      <h4 class="ui-bar-b">
        <strong>Indicator Details</strong>
      </h4>
      <div class="ui-grid-a">
        <div class="ui-block-a">
          Indicator 4 Name : <strong><xsl:value-of select="@TME2Name4"/></strong>
        </div>
        <div class="ui-block-b">
          Label : <xsl:value-of select="@TME2Label4"/>
        </div>
        <div class="ui-block-a">
          Date : <xsl:value-of select="@TME2Date4"/>
        </div>
        <div class="ui-block-b">
          Observations : <xsl:value-of select="@TME2N4"/>
        </div>
        <div class="ui-block-a">
          Unit : <xsl:value-of select="@TME2Unit4"/>
        </div>
        <div class="ui-block-b">
           Type : <xsl:value-of select="@TME2Type4"/>
        </div>
        <div class="ui-block-a">
          Total Planned Period : <xsl:value-of select="@TME2Total4"/>
        </div>
        <div class="ui-block-b">
          Total Plan Full : <xsl:value-of select="@TPFTotal4"/>
        </div>
        <div class="ui-block-a">
          Total Plan Cumul : <xsl:value-of select="@TPCTotal4"/>
        </div>
        <div class="ui-block-b">
          Total Actual Period : <xsl:value-of select="@TAPTotal4"/>
        </div>
        <div class="ui-block-a">
          Total Actual Cumul : <xsl:value-of select="@TACTotal4"/>
        </div>
        <div class="ui-block-b">
          Total Actual Period Change : <xsl:value-of select="@TAPChange4"/>
        </div>
        <div class="ui-block-a">
          Total Actual Cumul Change : <xsl:value-of select="@TACChange4"/>
        </div>
        <div class="ui-block-b">
          Total Planned Period Percent : <xsl:value-of select="@TPPPercent4"/>
        </div>
        <div class="ui-block-a">
          Total Planned Cumul Percent : <xsl:value-of select="@TPCPercent4"/>
        </div>
        <div class="ui-block-b">
          Total Planned Full Percent : <xsl:value-of select="@TPFPercent4"/>
        </div>
        <div class="ui-block-a">
          Q1 Unit : <xsl:value-of select="@TME2Q1Unit4"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Q1 Planned Period : <xsl:value-of select="@TME2Q1Total4"/>
        </div>
        <div class="ui-block-b">
          Q1 Plan Full : <xsl:value-of select="@TQ1PFTotal4"/>
        </div>
        <div class="ui-block-a">
          Q1 Plan Cumul : <xsl:value-of select="@TQ1PCTotal4"/>
        </div>
        <div class="ui-block-b">
          Q1 Actual Period : <xsl:value-of select="@TQ1APTotal4"/>
        </div>
        <div class="ui-block-a">
          Q1 Actual Cumul : <xsl:value-of select="@TQ1ACTotal4"/>
        </div>
        <div class="ui-block-b">
          Q1 Actual Period Change : <xsl:value-of select="@TQ1APChange4"/>
        </div>
        <div class="ui-block-a">
          Q1 Actual Cumul Change : <xsl:value-of select="@TQ1ACChange4"/>
        </div>
        <div class="ui-block-b">
          Q1 Planned Period Percent : <xsl:value-of select="@TQ1PPPercent4"/>
        </div>
        <div class="ui-block-a">
          Q1 Planned Cumul Percent : <xsl:value-of select="@TQ1PCPercent4"/>
        </div>
        <div class="ui-block-b">
          Q1 Planned Full Percent : <xsl:value-of select="@TQ1PFPercent4"/>
        </div>
        <div class="ui-block-a">
          Q2 Unit : <xsl:value-of select="@TME2Q2Unit4"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Q2 Planned Period : <xsl:value-of select="@TME2Q2Total4"/>
        </div>
        <div class="ui-block-b">
          Q2 Plan Full : <xsl:value-of select="@TQ2PFTotal4"/>
        </div>
        <div class="ui-block-a">
          Q2 Plan Cumul : <xsl:value-of select="@TQ2PCTotal4"/>
        </div>
        <div class="ui-block-b">
          Q2 Actual Period : <xsl:value-of select="@TQ2APTotal4"/>
        </div>
        <div class="ui-block-a">
          Q2 Actual Cumul : <xsl:value-of select="@TQ2ACTotal4"/>
        </div>
        <div class="ui-block-b">
          Q2 Actual Period Change : <xsl:value-of select="@TQ2APChange4"/>
        </div>
        <div class="ui-block-a">
          Q2 Actual Cumul Change : <xsl:value-of select="@TQ2ACChange4"/>
        </div>
        <div class="ui-block-b">
          Q2 Planned Period Percent : <xsl:value-of select="@TQ2PPPercent4"/>
        </div>
        <div class="ui-block-a">
          Q2 Planned Cumul Percent : <xsl:value-of select="@TQ2PCPercent4"/>
        </div>
        <div class="ui-block-b">
          Q2 Planned Full Percent : <xsl:value-of select="@TQ2PFPercent4"/>
        </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description4" />
	      </div>
      </div>
	</xsl:if>
  <xsl:if test="(@TME2Name5 != '')">
      <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
      <h4 class="ui-bar-b">
        <strong>Indicator Details</strong>
      </h4>
      <div class="ui-grid-a">
        <div class="ui-block-a">
          Indicator 5 Name : <strong><xsl:value-of select="@TME2Name5"/></strong>
        </div>
        <div class="ui-block-b">
          Label : <xsl:value-of select="@TME2Label5"/>
        </div>
        <div class="ui-block-a">
          Date : <xsl:value-of select="@TME2Date5"/>
        </div>
        <div class="ui-block-b">
          Observations : <xsl:value-of select="@TME2N5"/>
        </div>
        <div class="ui-block-a">
          Unit : <xsl:value-of select="@TME2Unit5"/>
        </div>
        <div class="ui-block-b">
           Type : <xsl:value-of select="@TME2Type5"/>
        </div>
        <div class="ui-block-a">
          Total Planned Period : <xsl:value-of select="@TME2Total5"/>
        </div>
        <div class="ui-block-b">
          Total Plan Full : <xsl:value-of select="@TPFTotal5"/>
        </div>
        <div class="ui-block-a">
          Total Plan Cumul : <xsl:value-of select="@TPCTotal5"/>
        </div>
        <div class="ui-block-b">
          Total Actual Period : <xsl:value-of select="@TAPTotal5"/>
        </div>
        <div class="ui-block-a">
          Total Actual Cumul : <xsl:value-of select="@TACTotal5"/>
        </div>
        <div class="ui-block-b">
          Total Actual Period Change : <xsl:value-of select="@TAPChange5"/>
        </div>
        <div class="ui-block-a">
          Total Actual Cumul Change : <xsl:value-of select="@TACChange5"/>
        </div>
        <div class="ui-block-b">
          Total Planned Period Percent : <xsl:value-of select="@TPPPercent5"/>
        </div>
        <div class="ui-block-a">
          Total Planned Cumul Percent : <xsl:value-of select="@TPCPercent5"/>
        </div>
        <div class="ui-block-b">
          Total Planned Full Percent : <xsl:value-of select="@TPFPercent5"/>
        </div>
        <div class="ui-block-a">
          Q1 Unit : <xsl:value-of select="@TME2Q1Unit5"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Q1 Planned Period : <xsl:value-of select="@TME2Q1Total5"/>
        </div>
        <div class="ui-block-b">
          Q1 Plan Full : <xsl:value-of select="@TQ1PFTotal5"/>
        </div>
        <div class="ui-block-a">
          Q1 Plan Cumul : <xsl:value-of select="@TQ1PCTotal5"/>
        </div>
        <div class="ui-block-b">
          Q1 Actual Period : <xsl:value-of select="@TQ1APTotal5"/>
        </div>
        <div class="ui-block-a">
          Q1 Actual Cumul : <xsl:value-of select="@TQ1ACTotal5"/>
        </div>
        <div class="ui-block-b">
          Q1 Actual Period Change : <xsl:value-of select="@TQ1APChange5"/>
        </div>
        <div class="ui-block-a">
          Q1 Actual Cumul Change : <xsl:value-of select="@TQ1ACChange5"/>
        </div>
        <div class="ui-block-b">
          Q1 Planned Period Percent : <xsl:value-of select="@TQ1PPPercent5"/>
        </div>
        <div class="ui-block-a">
          Q1 Planned Cumul Percent : <xsl:value-of select="@TQ1PCPercent5"/>
        </div>
        <div class="ui-block-b">
          Q1 Planned Full Percent : <xsl:value-of select="@TQ1PFPercent5"/>
        </div>
        <div class="ui-block-a">
          Q2 Unit : <xsl:value-of select="@TME2Q2Unit5"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Q2 Planned Period : <xsl:value-of select="@TME2Q2Total5"/>
        </div>
        <div class="ui-block-b">
          Q2 Plan Full : <xsl:value-of select="@TQ2PFTotal5"/>
        </div>
        <div class="ui-block-a">
          Q2 Plan Cumul : <xsl:value-of select="@TQ2PCTotal5"/>
        </div>
        <div class="ui-block-b">
          Q2 Actual Period : <xsl:value-of select="@TQ2APTotal5"/>
        </div>
        <div class="ui-block-a">
          Q2 Actual Cumul : <xsl:value-of select="@TQ2ACTotal5"/>
        </div>
        <div class="ui-block-b">
          Q2 Actual Period Change : <xsl:value-of select="@TQ2APChange5"/>
        </div>
        <div class="ui-block-a">
          Q2 Actual Cumul Change : <xsl:value-of select="@TQ2ACChange5"/>
        </div>
        <div class="ui-block-b">
          Q2 Planned Period Percent : <xsl:value-of select="@TQ2PPPercent5"/>
        </div>
        <div class="ui-block-a">
          Q2 Planned Cumul Percent : <xsl:value-of select="@TQ2PCPercent5"/>
        </div>
        <div class="ui-block-b">
          Q2 Planned Full Percent : <xsl:value-of select="@TQ2PFPercent5"/>
        </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description5" />
	      </div>
      </div>
	</xsl:if>
  <xsl:if test="(@TME2Name6 != '')">
      <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
      <h4 class="ui-bar-b">
        <strong>Indicator Details</strong>
      </h4>
      <div class="ui-grid-a">
        <div class="ui-block-a">
          Indicator 6 Name : <strong><xsl:value-of select="@TME2Name6"/></strong>
        </div>
        <div class="ui-block-b">
          Label : <xsl:value-of select="@TME2Label6"/>
        </div>
        <div class="ui-block-a">
          Date : <xsl:value-of select="@TME2Date6"/>
        </div>
        <div class="ui-block-b">
          Observations : <xsl:value-of select="@TME2N6"/>
        </div>
        <div class="ui-block-a">
          Unit : <xsl:value-of select="@TME2Unit6"/>
        </div>
        <div class="ui-block-b">
           Type : <xsl:value-of select="@TME2Type6"/>
        </div>
        <div class="ui-block-a">
          Total Planned Period : <xsl:value-of select="@TME2Total6"/>
        </div>
        <div class="ui-block-b">
          Total Plan Full : <xsl:value-of select="@TPFTotal6"/>
        </div>
        <div class="ui-block-a">
          Total Plan Cumul : <xsl:value-of select="@TPCTotal6"/>
        </div>
        <div class="ui-block-b">
          Total Actual Period : <xsl:value-of select="@TAPTotal6"/>
        </div>
        <div class="ui-block-a">
          Total Actual Cumul : <xsl:value-of select="@TACTotal6"/>
        </div>
        <div class="ui-block-b">
          Total Actual Period Change : <xsl:value-of select="@TAPChange6"/>
        </div>
        <div class="ui-block-a">
          Total Actual Cumul Change : <xsl:value-of select="@TACChange6"/>
        </div>
        <div class="ui-block-b">
          Total Planned Period Percent : <xsl:value-of select="@TPPPercent6"/>
        </div>
        <div class="ui-block-a">
          Total Planned Cumul Percent : <xsl:value-of select="@TPCPercent6"/>
        </div>
        <div class="ui-block-b">
          Total Planned Full Percent : <xsl:value-of select="@TPFPercent6"/>
        </div>
        <div class="ui-block-a">
          Q1 Unit : <xsl:value-of select="@TME2Q1Unit6"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Q1 Planned Period : <xsl:value-of select="@TME2Q1Total6"/>
        </div>
        <div class="ui-block-b">
          Q1 Plan Full : <xsl:value-of select="@TQ1PFTotal6"/>
        </div>
        <div class="ui-block-a">
          Q1 Plan Cumul : <xsl:value-of select="@TQ1PCTotal6"/>
        </div>
        <div class="ui-block-b">
          Q1 Actual Period : <xsl:value-of select="@TQ1APTotal6"/>
        </div>
        <div class="ui-block-a">
          Q1 Actual Cumul : <xsl:value-of select="@TQ1ACTotal6"/>
        </div>
        <div class="ui-block-b">
          Q1 Actual Period Change : <xsl:value-of select="@TQ1APChange6"/>
        </div>
        <div class="ui-block-a">
          Q1 Actual Cumul Change : <xsl:value-of select="@TQ1ACChange6"/>
        </div>
        <div class="ui-block-b">
          Q1 Planned Period Percent : <xsl:value-of select="@TQ1PPPercent6"/>
        </div>
        <div class="ui-block-a">
          Q1 Planned Cumul Percent : <xsl:value-of select="@TQ1PCPercent6"/>
        </div>
        <div class="ui-block-b">
          Q1 Planned Full Percent : <xsl:value-of select="@TQ1PFPercent6"/>
        </div>
        <div class="ui-block-a">
          Q2 Unit : <xsl:value-of select="@TME2Q2Unit6"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Q2 Planned Period : <xsl:value-of select="@TME2Q2Total6"/>
        </div>
        <div class="ui-block-b">
          Q2 Plan Full : <xsl:value-of select="@TQ2PFTotal6"/>
        </div>
        <div class="ui-block-a">
          Q2 Plan Cumul : <xsl:value-of select="@TQ2PCTotal6"/>
        </div>
        <div class="ui-block-b">
          Q2 Actual Period : <xsl:value-of select="@TQ2APTotal6"/>
        </div>
        <div class="ui-block-a">
          Q2 Actual Cumul : <xsl:value-of select="@TQ2ACTotal6"/>
        </div>
        <div class="ui-block-b">
          Q2 Actual Period Change : <xsl:value-of select="@TQ2APChange6"/>
        </div>
        <div class="ui-block-a">
          Q2 Actual Cumul Change : <xsl:value-of select="@TQ2ACChange6"/>
        </div>
        <div class="ui-block-b">
          Q2 Planned Period Percent : <xsl:value-of select="@TQ2PPPercent6"/>
        </div>
        <div class="ui-block-a">
          Q2 Planned Cumul Percent : <xsl:value-of select="@TQ2PCPercent6"/>
        </div>
        <div class="ui-block-b">
          Q2 Planned Full Percent : <xsl:value-of select="@TQ2PFPercent6"/>
        </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description6" />
	      </div>
      </div>
	</xsl:if>
  <xsl:if test="(@TME2Name7 != '')">
      <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
      <h4 class="ui-bar-b">
        <strong>Indicator Details</strong>
      </h4>
      <div class="ui-grid-a">
        <div class="ui-block-a">
          Indicator 7 Name : <strong><xsl:value-of select="@TME2Name7"/></strong>
        </div>
        <div class="ui-block-b">
          Label : <xsl:value-of select="@TME2Label7"/>
        </div>
        <div class="ui-block-a">
          Date : <xsl:value-of select="@TME2Date7"/>
        </div>
        <div class="ui-block-b">
          Observations : <xsl:value-of select="@TME2N7"/>
        </div>
        <div class="ui-block-a">
          Unit : <xsl:value-of select="@TME2Unit7"/>
        </div>
        <div class="ui-block-b">
           Type : <xsl:value-of select="@TME2Type7"/>
        </div>
        <div class="ui-block-a">
          Total Planned Period : <xsl:value-of select="@TME2Total7"/>
        </div>
        <div class="ui-block-b">
          Total Plan Full : <xsl:value-of select="@TPFTotal7"/>
        </div>
        <div class="ui-block-a">
          Total Plan Cumul : <xsl:value-of select="@TPCTotal7"/>
        </div>
        <div class="ui-block-b">
          Total Actual Period : <xsl:value-of select="@TAPTotal7"/>
        </div>
        <div class="ui-block-a">
          Total Actual Cumul : <xsl:value-of select="@TACTotal7"/>
        </div>
        <div class="ui-block-b">
          Total Actual Period Change : <xsl:value-of select="@TAPChange7"/>
        </div>
        <div class="ui-block-a">
          Total Actual Cumul Change : <xsl:value-of select="@TACChange7"/>
        </div>
        <div class="ui-block-b">
          Total Planned Period Percent : <xsl:value-of select="@TPPPercent7"/>
        </div>
        <div class="ui-block-a">
          Total Planned Cumul Percent : <xsl:value-of select="@TPCPercent7"/>
        </div>
        <div class="ui-block-b">
          Total Planned Full Percent : <xsl:value-of select="@TPFPercent7"/>
        </div>
        <div class="ui-block-a">
          Q1 Unit : <xsl:value-of select="@TME2Q1Unit7"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Q1 Planned Period : <xsl:value-of select="@TME2Q1Total7"/>
        </div>
        <div class="ui-block-b">
          Q1 Plan Full : <xsl:value-of select="@TQ1PFTotal7"/>
        </div>
        <div class="ui-block-a">
          Q1 Plan Cumul : <xsl:value-of select="@TQ1PCTotal7"/>
        </div>
        <div class="ui-block-b">
          Q1 Actual Period : <xsl:value-of select="@TQ1APTotal7"/>
        </div>
        <div class="ui-block-a">
          Q1 Actual Cumul : <xsl:value-of select="@TQ1ACTotal7"/>
        </div>
        <div class="ui-block-b">
          Q1 Actual Period Change : <xsl:value-of select="@TQ1APChange7"/>
        </div>
        <div class="ui-block-a">
          Q1 Actual Cumul Change : <xsl:value-of select="@TQ1ACChange7"/>
        </div>
        <div class="ui-block-b">
          Q1 Planned Period Percent : <xsl:value-of select="@TQ1PPPercent7"/>
        </div>
        <div class="ui-block-a">
          Q1 Planned Cumul Percent : <xsl:value-of select="@TQ1PCPercent7"/>
        </div>
        <div class="ui-block-b">
          Q1 Planned Full Percent : <xsl:value-of select="@TQ1PFPercent7"/>
        </div>
        <div class="ui-block-a">
          Q2 Unit : <xsl:value-of select="@TME2Q2Unit7"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Q2 Planned Period : <xsl:value-of select="@TME2Q2Total7"/>
        </div>
        <div class="ui-block-b">
          Q2 Plan Full : <xsl:value-of select="@TQ2PFTotal7"/>
        </div>
        <div class="ui-block-a">
          Q2 Plan Cumul : <xsl:value-of select="@TQ2PCTotal7"/>
        </div>
        <div class="ui-block-b">
          Q2 Actual Period : <xsl:value-of select="@TQ2APTotal7"/>
        </div>
        <div class="ui-block-a">
          Q2 Actual Cumul : <xsl:value-of select="@TQ2ACTotal7"/>
        </div>
        <div class="ui-block-b">
          Q2 Actual Period Change : <xsl:value-of select="@TQ2APChange7"/>
        </div>
        <div class="ui-block-a">
          Q2 Actual Cumul Change : <xsl:value-of select="@TQ2ACChange7"/>
        </div>
        <div class="ui-block-b">
          Q2 Planned Period Percent : <xsl:value-of select="@TQ2PPPercent7"/>
        </div>
        <div class="ui-block-a">
          Q2 Planned Cumul Percent : <xsl:value-of select="@TQ2PCPercent7"/>
        </div>
        <div class="ui-block-b">
          Q2 Planned Full Percent : <xsl:value-of select="@TQ2PFPercent7"/>
        </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description7" />
	      </div>
      </div>
	</xsl:if>
  <xsl:if test="(@TME2Name8 != '')">
      <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
      <h4 class="ui-bar-b">
        <strong>Indicator Details</strong>
      </h4>
      <div class="ui-grid-a">
        <div class="ui-block-a">
          Indicator 8 Name : <strong><xsl:value-of select="@TME2Name8"/></strong>
        </div>
        <div class="ui-block-b">
          Label : <xsl:value-of select="@TME2Label8"/>
        </div>
        <div class="ui-block-a">
          Date : <xsl:value-of select="@TME2Date8"/>
        </div>
        <div class="ui-block-b">
          Observations : <xsl:value-of select="@TME2N8"/>
        </div>
        <div class="ui-block-a">
          Unit : <xsl:value-of select="@TME2Unit8"/>
        </div>
        <div class="ui-block-b">
           Type : <xsl:value-of select="@TME2Type8"/>
        </div>
        <div class="ui-block-a">
          Total Planned Period : <xsl:value-of select="@TME2Total8"/>
        </div>
        <div class="ui-block-b">
          Total Plan Full : <xsl:value-of select="@TPFTotal8"/>
        </div>
        <div class="ui-block-a">
          Total Plan Cumul : <xsl:value-of select="@TPCTotal8"/>
        </div>
        <div class="ui-block-b">
          Total Actual Period : <xsl:value-of select="@TAPTotal8"/>
        </div>
        <div class="ui-block-a">
          Total Actual Cumul : <xsl:value-of select="@TACTotal8"/>
        </div>
        <div class="ui-block-b">
          Total Actual Period Change : <xsl:value-of select="@TAPChange8"/>
        </div>
        <div class="ui-block-a">
          Total Actual Cumul Change : <xsl:value-of select="@TACChange8"/>
        </div>
        <div class="ui-block-b">
          Total Planned Period Percent : <xsl:value-of select="@TPPPercent8"/>
        </div>
        <div class="ui-block-a">
          Total Planned Cumul Percent : <xsl:value-of select="@TPCPercent8"/>
        </div>
        <div class="ui-block-b">
          Total Planned Full Percent : <xsl:value-of select="@TPFPercent8"/>
        </div>
        <div class="ui-block-a">
          Q1 Unit : <xsl:value-of select="@TME2Q1Unit8"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Q1 Planned Period : <xsl:value-of select="@TME2Q1Total8"/>
        </div>
        <div class="ui-block-b">
          Q1 Plan Full : <xsl:value-of select="@TQ1PFTotal8"/>
        </div>
        <div class="ui-block-a">
          Q1 Plan Cumul : <xsl:value-of select="@TQ1PCTotal8"/>
        </div>
        <div class="ui-block-b">
          Q1 Actual Period : <xsl:value-of select="@TQ1APTotal8"/>
        </div>
        <div class="ui-block-a">
          Q1 Actual Cumul : <xsl:value-of select="@TQ1ACTotal8"/>
        </div>
        <div class="ui-block-b">
          Q1 Actual Period Change : <xsl:value-of select="@TQ1APChange8"/>
        </div>
        <div class="ui-block-a">
          Q1 Actual Cumul Change : <xsl:value-of select="@TQ1ACChange8"/>
        </div>
        <div class="ui-block-b">
          Q1 Planned Period Percent : <xsl:value-of select="@TQ1PPPercent8"/>
        </div>
        <div class="ui-block-a">
          Q1 Planned Cumul Percent : <xsl:value-of select="@TQ1PCPercent8"/>
        </div>
        <div class="ui-block-b">
          Q1 Planned Full Percent : <xsl:value-of select="@TQ1PFPercent8"/>
        </div>
        <div class="ui-block-a">
          Q2 Unit : <xsl:value-of select="@TME2Q2Unit8"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Q2 Planned Period : <xsl:value-of select="@TME2Q2Total8"/>
        </div>
        <div class="ui-block-b">
          Q2 Plan Full : <xsl:value-of select="@TQ2PFTotal8"/>
        </div>
        <div class="ui-block-a">
          Q2 Plan Cumul : <xsl:value-of select="@TQ2PCTotal8"/>
        </div>
        <div class="ui-block-b">
          Q2 Actual Period : <xsl:value-of select="@TQ2APTotal8"/>
        </div>
        <div class="ui-block-a">
          Q2 Actual Cumul : <xsl:value-of select="@TQ2ACTotal8"/>
        </div>
        <div class="ui-block-b">
          Q2 Actual Period Change : <xsl:value-of select="@TQ2APChange8"/>
        </div>
        <div class="ui-block-a">
          Q2 Actual Cumul Change : <xsl:value-of select="@TQ2ACChange8"/>
        </div>
        <div class="ui-block-b">
          Q2 Planned Period Percent : <xsl:value-of select="@TQ2PPPercent8"/>
        </div>
        <div class="ui-block-a">
          Q2 Planned Cumul Percent : <xsl:value-of select="@TQ2PCPercent8"/>
        </div>
        <div class="ui-block-b">
          Q2 Planned Full Percent : <xsl:value-of select="@TQ2PFPercent8"/>
        </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description8" />
	      </div>
      </div>
	</xsl:if>
  <xsl:if test="(@TME2Name9 != '')">
      <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
      <h4 class="ui-bar-b">
        <strong>Indicator Details</strong>
      </h4>
      <div class="ui-grid-a">
        <div class="ui-block-a">
          Indicator 9 Name : <strong><xsl:value-of select="@TME2Name9"/></strong>
        </div>
        <div class="ui-block-b">
          Label : <xsl:value-of select="@TME2Label9"/>
        </div>
        <div class="ui-block-a">
          Date : <xsl:value-of select="@TME2Date9"/>
        </div>
        <div class="ui-block-b">
          Observations : <xsl:value-of select="@TME2N9"/>
        </div>
        <div class="ui-block-a">
          Unit : <xsl:value-of select="@TME2Unit9"/>
        </div>
        <div class="ui-block-b">
           Type : <xsl:value-of select="@TME2Type9"/>
        </div>
        <div class="ui-block-a">
          Total Planned Period : <xsl:value-of select="@TME2Total9"/>
        </div>
        <div class="ui-block-b">
          Total Plan Full : <xsl:value-of select="@TPFTotal9"/>
        </div>
        <div class="ui-block-a">
          Total Plan Cumul : <xsl:value-of select="@TPCTotal9"/>
        </div>
        <div class="ui-block-b">
          Total Actual Period : <xsl:value-of select="@TAPTotal9"/>
        </div>
        <div class="ui-block-a">
          Total Actual Cumul : <xsl:value-of select="@TACTotal9"/>
        </div>
        <div class="ui-block-b">
          Total Actual Period Change : <xsl:value-of select="@TAPChange9"/>
        </div>
        <div class="ui-block-a">
          Total Actual Cumul Change : <xsl:value-of select="@TACChange9"/>
        </div>
        <div class="ui-block-b">
          Total Planned Period Percent : <xsl:value-of select="@TPPPercent9"/>
        </div>
        <div class="ui-block-a">
          Total Planned Cumul Percent : <xsl:value-of select="@TPCPercent9"/>
        </div>
        <div class="ui-block-b">
          Total Planned Full Percent : <xsl:value-of select="@TPFPercent9"/>
        </div>
        <div class="ui-block-a">
          Q1 Unit : <xsl:value-of select="@TME2Q1Unit9"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Q1 Planned Period : <xsl:value-of select="@TME2Q1Total9"/>
        </div>
        <div class="ui-block-b">
          Q1 Plan Full : <xsl:value-of select="@TQ1PFTotal9"/>
        </div>
        <div class="ui-block-a">
          Q1 Plan Cumul : <xsl:value-of select="@TQ1PCTotal9"/>
        </div>
        <div class="ui-block-b">
          Q1 Actual Period : <xsl:value-of select="@TQ1APTotal9"/>
        </div>
        <div class="ui-block-a">
          Q1 Actual Cumul : <xsl:value-of select="@TQ1ACTotal9"/>
        </div>
        <div class="ui-block-b">
          Q1 Actual Period Change : <xsl:value-of select="@TQ1APChange9"/>
        </div>
        <div class="ui-block-a">
          Q1 Actual Cumul Change : <xsl:value-of select="@TQ1ACChange9"/>
        </div>
        <div class="ui-block-b">
          Q1 Planned Period Percent : <xsl:value-of select="@TQ1PPPercent9"/>
        </div>
        <div class="ui-block-a">
          Q1 Planned Cumul Percent : <xsl:value-of select="@TQ1PCPercent9"/>
        </div>
        <div class="ui-block-b">
          Q1 Planned Full Percent : <xsl:value-of select="@TQ1PFPercent9"/>
        </div>
        <div class="ui-block-a">
          Q2 Unit : <xsl:value-of select="@TME2Q2Unit9"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Q2 Planned Period : <xsl:value-of select="@TME2Q2Total9"/>
        </div>
        <div class="ui-block-b">
          Q2 Plan Full : <xsl:value-of select="@TQ2PFTotal9"/>
        </div>
        <div class="ui-block-a">
          Q2 Plan Cumul : <xsl:value-of select="@TQ2PCTotal9"/>
        </div>
        <div class="ui-block-b">
          Q2 Actual Period : <xsl:value-of select="@TQ2APTotal9"/>
        </div>
        <div class="ui-block-a">
          Q2 Actual Cumul : <xsl:value-of select="@TQ2ACTotal9"/>
        </div>
        <div class="ui-block-b">
          Q2 Actual Period Change : <xsl:value-of select="@TQ2APChange9"/>
        </div>
        <div class="ui-block-a">
          Q2 Actual Cumul Change : <xsl:value-of select="@TQ2ACChange9"/>
        </div>
        <div class="ui-block-b">
          Q2 Planned Period Percent : <xsl:value-of select="@TQ2PPPercent9"/>
        </div>
        <div class="ui-block-a">
          Q2 Planned Cumul Percent : <xsl:value-of select="@TQ2PCPercent9"/>
        </div>
        <div class="ui-block-b">
          Q2 Planned Full Percent : <xsl:value-of select="@TQ2PFPercent9"/>
        </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description9" />
	      </div>
      </div>
	</xsl:if>
  <xsl:if test="(@TME2Name10 != '')">
      <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
      <h4 class="ui-bar-b">
        <strong>Indicator Details</strong>
      </h4>
      <div class="ui-grid-a">
        <div class="ui-block-a">
          Indicator 10 Name : <strong><xsl:value-of select="@TME2Name10"/></strong>
        </div>
        <div class="ui-block-b">
          Label : <xsl:value-of select="@TME2Label10"/>
        </div>
        <div class="ui-block-a">
          Date : <xsl:value-of select="@TME2Date10"/>
        </div>
        <div class="ui-block-b">
          Observations : <xsl:value-of select="@TME2N10"/>
        </div>
        <div class="ui-block-a">
          Unit : <xsl:value-of select="@TME2Unit10"/>
        </div>
        <div class="ui-block-b">
           Type : <xsl:value-of select="@TME2Type10"/>
        </div>
        <div class="ui-block-a">
          Total Planned Period : <xsl:value-of select="@TME2Total10"/>
        </div>
        <div class="ui-block-b">
          Total Plan Full : <xsl:value-of select="@TPFTotal10"/>
        </div>
        <div class="ui-block-a">
          Total Plan Cumul : <xsl:value-of select="@TPCTotal10"/>
        </div>
        <div class="ui-block-b">
          Total Actual Period : <xsl:value-of select="@TAPTotal10"/>
        </div>
        <div class="ui-block-a">
          Total Actual Cumul : <xsl:value-of select="@TACTotal10"/>
        </div>
        <div class="ui-block-b">
          Total Actual Period Change : <xsl:value-of select="@TAPChange10"/>
        </div>
        <div class="ui-block-a">
          Total Actual Cumul Change : <xsl:value-of select="@TACChange10"/>
        </div>
        <div class="ui-block-b">
          Total Planned Period Percent : <xsl:value-of select="@TPPPercent10"/>
        </div>
        <div class="ui-block-a">
          Total Planned Cumul Percent : <xsl:value-of select="@TPCPercent10"/>
        </div>
        <div class="ui-block-b">
          Total Planned Full Percent : <xsl:value-of select="@TPFPercent10"/>
        </div>
        <div class="ui-block-a">
          Q1 Unit : <xsl:value-of select="@TME2Q1Unit10"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Q1 Planned Period : <xsl:value-of select="@TME2Q1Total10"/>
        </div>
        <div class="ui-block-b">
          Q1 Plan Full : <xsl:value-of select="@TQ1PFTotal10"/>
        </div>
        <div class="ui-block-a">
          Q1 Plan Cumul : <xsl:value-of select="@TQ1PCTotal10"/>
        </div>
        <div class="ui-block-b">
          Q1 Actual Period : <xsl:value-of select="@TQ1APTotal10"/>
        </div>
        <div class="ui-block-a">
          Q1 Actual Cumul : <xsl:value-of select="@TQ1ACTotal10"/>
        </div>
        <div class="ui-block-b">
          Q1 Actual Period Change : <xsl:value-of select="@TQ1APChange10"/>
        </div>
        <div class="ui-block-a">
          Q1 Actual Cumul Change : <xsl:value-of select="@TQ1ACChange10"/>
        </div>
        <div class="ui-block-b">
          Q1 Planned Period Percent : <xsl:value-of select="@TQ1PPPercent10"/>
        </div>
        <div class="ui-block-a">
          Q1 Planned Cumul Percent : <xsl:value-of select="@TQ1PCPercent10"/>
        </div>
        <div class="ui-block-b">
          Q1 Planned Full Percent : <xsl:value-of select="@TQ1PFPercent10"/>
        </div>
        <div class="ui-block-a">
          Q2 Unit : <xsl:value-of select="@TME2Q2Unit10"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Q2 Planned Period : <xsl:value-of select="@TME2Q2Total10"/>
        </div>
        <div class="ui-block-b">
          Q2 Plan Full : <xsl:value-of select="@TQ2PFTotal10"/>
        </div>
        <div class="ui-block-a">
          Q2 Plan Cumul : <xsl:value-of select="@TQ2PCTotal10"/>
        </div>
        <div class="ui-block-b">
          Q2 Actual Period : <xsl:value-of select="@TQ2APTotal10"/>
        </div>
        <div class="ui-block-a">
          Q2 Actual Cumul : <xsl:value-of select="@TQ2ACTotal10"/>
        </div>
        <div class="ui-block-b">
          Q2 Actual Period Change : <xsl:value-of select="@TQ2APChange10"/>
        </div>
        <div class="ui-block-a">
          Q2 Actual Cumul Change : <xsl:value-of select="@TQ2ACChange10"/>
        </div>
        <div class="ui-block-b">
          Q2 Planned Period Percent : <xsl:value-of select="@TQ2PPPercent10"/>
        </div>
        <div class="ui-block-a">
          Q2 Planned Cumul Percent : <xsl:value-of select="@TQ2PCPercent10"/>
        </div>
        <div class="ui-block-b">
          Q2 Planned Full Percent : <xsl:value-of select="@TQ2PFPercent10"/>
        </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description10" />
	      </div>
      </div>
	</xsl:if>
	</xsl:template>
</xsl:stylesheet>