<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2016, October -->
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
		<div id="modEdits_divEditsDoc">
      <xsl:apply-templates select="servicebase" />
			<xsl:apply-templates select="investmentgroup" />
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
		<xsl:apply-templates select="investmentgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentgroup">
    <h4>
      <strong>Investment Group</strong>&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='meprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="investment">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investment">
		<h4>
      <strong>Investment</strong> :&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='meprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="investmenttimeperiod">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmenttimeperiod">
		<h4>
      <strong>Time Period</strong> :&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" />
    </h4>
    <xsl:apply-templates select="root/linkedview[@AnalyzerType='meprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
		<xsl:apply-templates select="investmentoutcomes" />
		<xsl:apply-templates select="investmentcomponents" />
	</xsl:template>
	<xsl:template match="investmentoutcomes">
		<xsl:apply-templates select="investmentoutcome">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentoutcome">
		<h4>
      <strong>Outcome </strong> :&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='meprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	<xsl:apply-templates select="investmentoutput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentoutput">
    <h4>
      <strong>Output </strong> :&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='meprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentcomponents">
		<xsl:apply-templates select="investmentcomponent">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentcomponent">
		<h4>
      <strong>Component</strong> :&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='meprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	<xsl:apply-templates select="investmentinput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentinput">
		<h4>
      <strong>Input</strong> :&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" />
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
    <xsl:if test="(@TME2Name0 != '' and @TME2Name0 != 'none')">
      <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
      <h4 class="ui-bar-b">
        <strong>Indicator Details</strong>
      </h4>
      <div class="ui-grid-a">
        <div class="ui-block-a">
          Indicator 0 Name : <strong><xsl:value-of select="@TME2Name0"/></strong>
        </div>
        <div class="ui-block-b">
          Label : <xsl:value-of select="@TME2Label0"/>
        </div>
        <div class="ui-block-a">
          Date : <xsl:value-of select="@TME2Date0"/>
        </div>
        <div class="ui-block-b">
          Observations : <xsl:value-of select="@TME2N0"/>
        </div>
        <div class="ui-block-a">
          Unit : <xsl:value-of select="@TME2TMUnit0"/>
        </div>
        <div class="ui-block-b">
           Type : <xsl:value-of select="@TME2Type0"/>
        </div>
        <div class="ui-block-a">
          Most Planned Period : <xsl:value-of select="@TME2TMAmount0"/>
        </div>
        <div class="ui-block-b">
          Most Plan Full : <xsl:value-of select="@TMPFTotal0"/>
        </div>
        <div class="ui-block-a">
          Most Plan Cumul : <xsl:value-of select="@TMPCTotal0"/>
        </div>
        <div class="ui-block-b">
          Most Actual Period : <xsl:value-of select="@TMAPTotal0"/>
        </div>
        <div class="ui-block-a">
          Most Actual Cumul : <xsl:value-of select="@TMACTotal0"/>
        </div>
        <div class="ui-block-b">
          Most Actual Period Change : <xsl:value-of select="@TMAPChange0"/>
        </div>
        <div class="ui-block-a">
          Most Actual Cumul Change : <xsl:value-of select="@TMACChange0"/>
        </div>
        <div class="ui-block-b">
          Most Planned Period Percent : <xsl:value-of select="@TMPPPercent0"/>
        </div>
        <div class="ui-block-a">
          Most Planned Cumul Percent : <xsl:value-of select="@TMPCPercent0"/>
        </div>
        <div class="ui-block-b">
          Most Planned Full Percent : <xsl:value-of select="@TMPFPercent0"/>
        </div>
        <div class="ui-block-a">
          L Unit : <xsl:value-of select="@TME2TLUnit0"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          L Planned Period : <xsl:value-of select="@TME2TLAmount0"/>
        </div>
        <div class="ui-block-b">
          L Plan Full : <xsl:value-of select="@TLPFTotal0"/>
        </div>
        <div class="ui-block-a">
          L Plan Cumul : <xsl:value-of select="@TLPCTotal0"/>
        </div>
        <div class="ui-block-b">
          L Actual Period : <xsl:value-of select="@TLAPTotal0"/>
        </div>
        <div class="ui-block-a">
          L Actual Cumul : <xsl:value-of select="@TLACTotal0"/>
        </div>
        <div class="ui-block-b">
          L Actual Period Change : <xsl:value-of select="@TLAPChange0"/>
        </div>
        <div class="ui-block-a">
          L Actual Cumul Change : <xsl:value-of select="@TLACChange0"/>
        </div>
        <div class="ui-block-b">
          L Planned Period Percent : <xsl:value-of select="@TLPPPercent0"/>
        </div>
        <div class="ui-block-a">
          L Planned Cumul Percent : <xsl:value-of select="@TLPCPercent0"/>
        </div>
        <div class="ui-block-b">
          L Planned Full Percent : <xsl:value-of select="@TLPFPercent0"/>
        </div>
        <div class="ui-block-a">
          U Unit : <xsl:value-of select="@TME2TUUnit0"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          U Planned Period : <xsl:value-of select="@TME2TUAmount0"/>
        </div>
        <div class="ui-block-b">
          U Plan Full : <xsl:value-of select="@TUPFTotal0"/>
        </div>
        <div class="ui-block-a">
          U Plan Cumul : <xsl:value-of select="@TUPCTotal0"/>
        </div>
        <div class="ui-block-b">
          U Actual Period : <xsl:value-of select="@TUAPTotal0"/>
        </div>
        <div class="ui-block-a">
          U Actual Cumul : <xsl:value-of select="@TUACTotal0"/>
        </div>
        <div class="ui-block-b">
          U Actual Period Change : <xsl:value-of select="@TUAPChange0"/>
        </div>
        <div class="ui-block-a">
          U Actual Cumul Change : <xsl:value-of select="@TUACChange0"/>
        </div>
        <div class="ui-block-b">
          U Planned Period Percent : <xsl:value-of select="@TUPPPercent0"/>
        </div>
        <div class="ui-block-a">
          U Planned Cumul Percent : <xsl:value-of select="@TUPCPercent0"/>
        </div>
        <div class="ui-block-b">
          U Planned Full Percent : <xsl:value-of select="@TUPFPercent0"/>
        </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description0" />
	      </div>
      </div>
	</xsl:if>
    <xsl:if test="(@TME2Name1 != '' and @TME2Name1 != 'none')">
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
          Unit : <xsl:value-of select="@TME2TMUnit1"/>
        </div>
        <div class="ui-block-b">
           Type : <xsl:value-of select="@TME2Type1"/>
        </div>
        <div class="ui-block-a">
          Most Planned Period : <xsl:value-of select="@TME2TMAmount1"/>
        </div>
        <div class="ui-block-b">
          Most Plan Full : <xsl:value-of select="@TMPFTotal1"/>
        </div>
        <div class="ui-block-a">
          Most Plan Cumul : <xsl:value-of select="@TMPCTotal1"/>
        </div>
        <div class="ui-block-b">
          Most Actual Period : <xsl:value-of select="@TMAPTotal1"/>
        </div>
        <div class="ui-block-a">
          Most Actual Cumul : <xsl:value-of select="@TMACTotal1"/>
        </div>
        <div class="ui-block-b">
          Most Actual Period Change : <xsl:value-of select="@TMAPChange1"/>
        </div>
        <div class="ui-block-a">
          Most Actual Cumul Change : <xsl:value-of select="@TMACChange1"/>
        </div>
        <div class="ui-block-b">
          Most Planned Period Percent : <xsl:value-of select="@TMPPPercent1"/>
        </div>
        <div class="ui-block-a">
          Most Planned Cumul Percent : <xsl:value-of select="@TMPCPercent1"/>
        </div>
        <div class="ui-block-b">
          Most Planned Full Percent : <xsl:value-of select="@TMPFPercent1"/>
        </div>
        <div class="ui-block-a">
          L Unit : <xsl:value-of select="@TME2TLUnit1"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          L Planned Period : <xsl:value-of select="@TME2TLAmount1"/>
        </div>
        <div class="ui-block-b">
          L Plan Full : <xsl:value-of select="@TLPFTotal1"/>
        </div>
        <div class="ui-block-a">
          L Plan Cumul : <xsl:value-of select="@TLPCTotal1"/>
        </div>
        <div class="ui-block-b">
          L Actual Period : <xsl:value-of select="@TLAPTotal1"/>
        </div>
        <div class="ui-block-a">
          L Actual Cumul : <xsl:value-of select="@TLACTotal1"/>
        </div>
        <div class="ui-block-b">
          L Actual Period Change : <xsl:value-of select="@TLAPChange1"/>
        </div>
        <div class="ui-block-a">
          L Actual Cumul Change : <xsl:value-of select="@TLACChange1"/>
        </div>
        <div class="ui-block-b">
          L Planned Period Percent : <xsl:value-of select="@TLPPPercent1"/>
        </div>
        <div class="ui-block-a">
          L Planned Cumul Percent : <xsl:value-of select="@TLPCPercent1"/>
        </div>
        <div class="ui-block-b">
          L Planned Full Percent : <xsl:value-of select="@TLPFPercent1"/>
        </div>
        <div class="ui-block-a">
          U Unit : <xsl:value-of select="@TME2TUUnit1"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          U Planned Period : <xsl:value-of select="@TME2TUAmount1"/>
        </div>
        <div class="ui-block-b">
          U Plan Full : <xsl:value-of select="@TUPFTotal1"/>
        </div>
        <div class="ui-block-a">
          U Plan Cumul : <xsl:value-of select="@TUPCTotal1"/>
        </div>
        <div class="ui-block-b">
          U Actual Period : <xsl:value-of select="@TUAPTotal1"/>
        </div>
        <div class="ui-block-a">
          U Actual Cumul : <xsl:value-of select="@TUACTotal1"/>
        </div>
        <div class="ui-block-b">
          U Actual Period Change : <xsl:value-of select="@TUAPChange1"/>
        </div>
        <div class="ui-block-a">
          U Actual Cumul Change : <xsl:value-of select="@TUACChange1"/>
        </div>
        <div class="ui-block-b">
          U Planned Period Percent : <xsl:value-of select="@TUPPPercent1"/>
        </div>
        <div class="ui-block-a">
          U Planned Cumul Percent : <xsl:value-of select="@TUPCPercent1"/>
        </div>
        <div class="ui-block-b">
          U Planned Full Percent : <xsl:value-of select="@TUPFPercent1"/>
        </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description1" />
	      </div>
      </div>
	</xsl:if>
  <xsl:if test="(@TME2Name2 != '' and @TME2Name2 != 'none')">
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
          Unit : <xsl:value-of select="@TME2TMUnit2"/>
        </div>
        <div class="ui-block-b">
           Type : <xsl:value-of select="@TME2Type2"/>
        </div>
        <div class="ui-block-a">
          Most Planned Period : <xsl:value-of select="@TME2TMAmount2"/>
        </div>
        <div class="ui-block-b">
          Most Plan Full : <xsl:value-of select="@TMPFTotal2"/>
        </div>
        <div class="ui-block-a">
          Most Plan Cumul : <xsl:value-of select="@TMPCTotal2"/>
        </div>
        <div class="ui-block-b">
          Most Actual Period : <xsl:value-of select="@TMAPTotal2"/>
        </div>
        <div class="ui-block-a">
          Most Actual Cumul : <xsl:value-of select="@TMACTotal2"/>
        </div>
        <div class="ui-block-b">
          Most Actual Period Change : <xsl:value-of select="@TMAPChange2"/>
        </div>
        <div class="ui-block-a">
          Most Actual Cumul Change : <xsl:value-of select="@TMACChange2"/>
        </div>
        <div class="ui-block-b">
          Most Planned Period Percent : <xsl:value-of select="@TMPPPercent2"/>
        </div>
        <div class="ui-block-a">
          Most Planned Cumul Percent : <xsl:value-of select="@TMPCPercent2"/>
        </div>
        <div class="ui-block-b">
          Most Planned Full Percent : <xsl:value-of select="@TMPFPercent2"/>
        </div>
        <div class="ui-block-a">
          L Unit : <xsl:value-of select="@TME2TLUnit2"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          L Planned Period : <xsl:value-of select="@TME2TLAmount2"/>
        </div>
        <div class="ui-block-b">
          L Plan Full : <xsl:value-of select="@TLPFTotal2"/>
        </div>
        <div class="ui-block-a">
          L Plan Cumul : <xsl:value-of select="@TLPCTotal2"/>
        </div>
        <div class="ui-block-b">
          L Actual Period : <xsl:value-of select="@TLAPTotal2"/>
        </div>
        <div class="ui-block-a">
          L Actual Cumul : <xsl:value-of select="@TLACTotal2"/>
        </div>
        <div class="ui-block-b">
          L Actual Period Change : <xsl:value-of select="@TLAPChange2"/>
        </div>
        <div class="ui-block-a">
          L Actual Cumul Change : <xsl:value-of select="@TLACChange2"/>
        </div>
        <div class="ui-block-b">
          L Planned Period Percent : <xsl:value-of select="@TLPPPercent2"/>
        </div>
        <div class="ui-block-a">
          L Planned Cumul Percent : <xsl:value-of select="@TLPCPercent2"/>
        </div>
        <div class="ui-block-b">
          L Planned Full Percent : <xsl:value-of select="@TLPFPercent2"/>
        </div>
        <div class="ui-block-a">
          U Unit : <xsl:value-of select="@TME2TUUnit2"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          U Planned Period : <xsl:value-of select="@TME2TUAmount2"/>
        </div>
        <div class="ui-block-b">
          U Plan Full : <xsl:value-of select="@TUPFTotal2"/>
        </div>
        <div class="ui-block-a">
          U Plan Cumul : <xsl:value-of select="@TUPCTotal2"/>
        </div>
        <div class="ui-block-b">
          U Actual Period : <xsl:value-of select="@TUAPTotal2"/>
        </div>
        <div class="ui-block-a">
          U Actual Cumul : <xsl:value-of select="@TUACTotal2"/>
        </div>
        <div class="ui-block-b">
          U Actual Period Change : <xsl:value-of select="@TUAPChange2"/>
        </div>
        <div class="ui-block-a">
          U Actual Cumul Change : <xsl:value-of select="@TUACChange2"/>
        </div>
        <div class="ui-block-b">
          U Planned Period Percent : <xsl:value-of select="@TUPPPercent2"/>
        </div>
        <div class="ui-block-a">
          U Planned Cumul Percent : <xsl:value-of select="@TUPCPercent2"/>
        </div>
        <div class="ui-block-b">
          U Planned Full Percent : <xsl:value-of select="@TUPFPercent2"/>
        </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description2" />
	      </div>
      </div>
	</xsl:if>
  <xsl:if test="(@TME2Name3 != '' and @TME2Name3 != 'none')">
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
          Unit : <xsl:value-of select="@TME2TMUnit3"/>
        </div>
        <div class="ui-block-b">
           Type : <xsl:value-of select="@TME2Type3"/>
        </div>
        <div class="ui-block-a">
          Most Planned Period : <xsl:value-of select="@TME2TMAmount3"/>
        </div>
        <div class="ui-block-b">
          Most Plan Full : <xsl:value-of select="@TMPFTotal3"/>
        </div>
        <div class="ui-block-a">
          Most Plan Cumul : <xsl:value-of select="@TMPCTotal3"/>
        </div>
        <div class="ui-block-b">
          Most Actual Period : <xsl:value-of select="@TMAPTotal3"/>
        </div>
        <div class="ui-block-a">
          Most Actual Cumul : <xsl:value-of select="@TMACTotal3"/>
        </div>
        <div class="ui-block-b">
          Most Actual Period Change : <xsl:value-of select="@TMAPChange3"/>
        </div>
        <div class="ui-block-a">
          Most Actual Cumul Change : <xsl:value-of select="@TMACChange3"/>
        </div>
        <div class="ui-block-b">
          Most Planned Period Percent : <xsl:value-of select="@TMPPPercent3"/>
        </div>
        <div class="ui-block-a">
          Most Planned Cumul Percent : <xsl:value-of select="@TMPCPercent3"/>
        </div>
        <div class="ui-block-b">
          Most Planned Full Percent : <xsl:value-of select="@TMPFPercent3"/>
        </div>
        <div class="ui-block-a">
          L Unit : <xsl:value-of select="@TME2TLUnit3"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          L Planned Period : <xsl:value-of select="@TME2TLAmount3"/>
        </div>
        <div class="ui-block-b">
          L Plan Full : <xsl:value-of select="@TLPFTotal3"/>
        </div>
        <div class="ui-block-a">
          L Plan Cumul : <xsl:value-of select="@TLPCTotal3"/>
        </div>
        <div class="ui-block-b">
          L Actual Period : <xsl:value-of select="@TLAPTotal3"/>
        </div>
        <div class="ui-block-a">
          L Actual Cumul : <xsl:value-of select="@TLACTotal3"/>
        </div>
        <div class="ui-block-b">
          L Actual Period Change : <xsl:value-of select="@TLAPChange3"/>
        </div>
        <div class="ui-block-a">
          L Actual Cumul Change : <xsl:value-of select="@TLACChange3"/>
        </div>
        <div class="ui-block-b">
          L Planned Period Percent : <xsl:value-of select="@TLPPPercent3"/>
        </div>
        <div class="ui-block-a">
          L Planned Cumul Percent : <xsl:value-of select="@TLPCPercent3"/>
        </div>
        <div class="ui-block-b">
          L Planned Full Percent : <xsl:value-of select="@TLPFPercent3"/>
        </div>
        <div class="ui-block-a">
          U Unit : <xsl:value-of select="@TME2TUUnit3"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          U Planned Period : <xsl:value-of select="@TME2TUAmount3"/>
        </div>
        <div class="ui-block-b">
          U Plan Full : <xsl:value-of select="@TUPFTotal3"/>
        </div>
        <div class="ui-block-a">
          U Plan Cumul : <xsl:value-of select="@TUPCTotal3"/>
        </div>
        <div class="ui-block-b">
          U Actual Period : <xsl:value-of select="@TUAPTotal3"/>
        </div>
        <div class="ui-block-a">
          U Actual Cumul : <xsl:value-of select="@TUACTotal3"/>
        </div>
        <div class="ui-block-b">
          U Actual Period Change : <xsl:value-of select="@TUAPChange3"/>
        </div>
        <div class="ui-block-a">
          U Actual Cumul Change : <xsl:value-of select="@TUACChange3"/>
        </div>
        <div class="ui-block-b">
          U Planned Period Percent : <xsl:value-of select="@TUPPPercent3"/>
        </div>
        <div class="ui-block-a">
          U Planned Cumul Percent : <xsl:value-of select="@TUPCPercent3"/>
        </div>
        <div class="ui-block-b">
          U Planned Full Percent : <xsl:value-of select="@TUPFPercent3"/>
        </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description3" />
	      </div>
      </div>
	</xsl:if>
  <xsl:if test="(@TME2Name4 != '' and @TME2Name4 != 'none')">
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
          Unit : <xsl:value-of select="@TME2TMUnit4"/>
        </div>
        <div class="ui-block-b">
           Type : <xsl:value-of select="@TME2Type4"/>
        </div>
        <div class="ui-block-a">
          Most Planned Period : <xsl:value-of select="@TME2TMAmount4"/>
        </div>
        <div class="ui-block-b">
          Most Plan Full : <xsl:value-of select="@TMPFTotal4"/>
        </div>
        <div class="ui-block-a">
          Most Plan Cumul : <xsl:value-of select="@TMPCTotal4"/>
        </div>
        <div class="ui-block-b">
          Most Actual Period : <xsl:value-of select="@TMAPTotal4"/>
        </div>
        <div class="ui-block-a">
          Most Actual Cumul : <xsl:value-of select="@TMACTotal4"/>
        </div>
        <div class="ui-block-b">
          Most Actual Period Change : <xsl:value-of select="@TMAPChange4"/>
        </div>
        <div class="ui-block-a">
          Most Actual Cumul Change : <xsl:value-of select="@TMACChange4"/>
        </div>
        <div class="ui-block-b">
          Most Planned Period Percent : <xsl:value-of select="@TMPPPercent4"/>
        </div>
        <div class="ui-block-a">
          Most Planned Cumul Percent : <xsl:value-of select="@TMPCPercent4"/>
        </div>
        <div class="ui-block-b">
          Most Planned Full Percent : <xsl:value-of select="@TMPFPercent4"/>
        </div>
        <div class="ui-block-a">
          L Unit : <xsl:value-of select="@TME2TLUnit4"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          L Planned Period : <xsl:value-of select="@TME2TLAmount4"/>
        </div>
        <div class="ui-block-b">
          L Plan Full : <xsl:value-of select="@TLPFTotal4"/>
        </div>
        <div class="ui-block-a">
          L Plan Cumul : <xsl:value-of select="@TLPCTotal4"/>
        </div>
        <div class="ui-block-b">
          L Actual Period : <xsl:value-of select="@TLAPTotal4"/>
        </div>
        <div class="ui-block-a">
          L Actual Cumul : <xsl:value-of select="@TLACTotal4"/>
        </div>
        <div class="ui-block-b">
          L Actual Period Change : <xsl:value-of select="@TLAPChange4"/>
        </div>
        <div class="ui-block-a">
          L Actual Cumul Change : <xsl:value-of select="@TLACChange4"/>
        </div>
        <div class="ui-block-b">
          L Planned Period Percent : <xsl:value-of select="@TLPPPercent4"/>
        </div>
        <div class="ui-block-a">
          L Planned Cumul Percent : <xsl:value-of select="@TLPCPercent4"/>
        </div>
        <div class="ui-block-b">
          L Planned Full Percent : <xsl:value-of select="@TLPFPercent4"/>
        </div>
        <div class="ui-block-a">
          U Unit : <xsl:value-of select="@TME2TUUnit4"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          U Planned Period : <xsl:value-of select="@TME2TUAmount4"/>
        </div>
        <div class="ui-block-b">
          U Plan Full : <xsl:value-of select="@TUPFTotal4"/>
        </div>
        <div class="ui-block-a">
          U Plan Cumul : <xsl:value-of select="@TUPCTotal4"/>
        </div>
        <div class="ui-block-b">
          U Actual Period : <xsl:value-of select="@TUAPTotal4"/>
        </div>
        <div class="ui-block-a">
          U Actual Cumul : <xsl:value-of select="@TUACTotal4"/>
        </div>
        <div class="ui-block-b">
          U Actual Period Change : <xsl:value-of select="@TUAPChange4"/>
        </div>
        <div class="ui-block-a">
          U Actual Cumul Change : <xsl:value-of select="@TUACChange4"/>
        </div>
        <div class="ui-block-b">
          U Planned Period Percent : <xsl:value-of select="@TUPPPercent4"/>
        </div>
        <div class="ui-block-a">
          U Planned Cumul Percent : <xsl:value-of select="@TUPCPercent4"/>
        </div>
        <div class="ui-block-b">
          U Planned Full Percent : <xsl:value-of select="@TUPFPercent4"/>
        </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description4" />
	      </div>
      </div>
	</xsl:if>
  <xsl:if test="(@TME2Name5 != '' and @TME2Name5 != 'none')">
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
          Unit : <xsl:value-of select="@TME2TMUnit5"/>
        </div>
        <div class="ui-block-b">
           Type : <xsl:value-of select="@TME2Type5"/>
        </div>
        <div class="ui-block-a">
          Most Planned Period : <xsl:value-of select="@TME2TMAmount5"/>
        </div>
        <div class="ui-block-b">
          Most Plan Full : <xsl:value-of select="@TMPFTotal5"/>
        </div>
        <div class="ui-block-a">
          Most Plan Cumul : <xsl:value-of select="@TMPCTotal5"/>
        </div>
        <div class="ui-block-b">
          Most Actual Period : <xsl:value-of select="@TMAPTotal5"/>
        </div>
        <div class="ui-block-a">
          Most Actual Cumul : <xsl:value-of select="@TMACTotal5"/>
        </div>
        <div class="ui-block-b">
          Most Actual Period Change : <xsl:value-of select="@TMAPChange5"/>
        </div>
        <div class="ui-block-a">
          Most Actual Cumul Change : <xsl:value-of select="@TMACChange5"/>
        </div>
        <div class="ui-block-b">
          Most Planned Period Percent : <xsl:value-of select="@TMPPPercent5"/>
        </div>
        <div class="ui-block-a">
          Most Planned Cumul Percent : <xsl:value-of select="@TMPCPercent5"/>
        </div>
        <div class="ui-block-b">
          Most Planned Full Percent : <xsl:value-of select="@TMPFPercent5"/>
        </div>
        <div class="ui-block-a">
          L Unit : <xsl:value-of select="@TME2TLUnit5"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          L Planned Period : <xsl:value-of select="@TME2TLAmount5"/>
        </div>
        <div class="ui-block-b">
          L Plan Full : <xsl:value-of select="@TLPFTotal5"/>
        </div>
        <div class="ui-block-a">
          L Plan Cumul : <xsl:value-of select="@TLPCTotal5"/>
        </div>
        <div class="ui-block-b">
          L Actual Period : <xsl:value-of select="@TLAPTotal5"/>
        </div>
        <div class="ui-block-a">
          L Actual Cumul : <xsl:value-of select="@TLACTotal5"/>
        </div>
        <div class="ui-block-b">
          L Actual Period Change : <xsl:value-of select="@TLAPChange5"/>
        </div>
        <div class="ui-block-a">
          L Actual Cumul Change : <xsl:value-of select="@TLACChange5"/>
        </div>
        <div class="ui-block-b">
          L Planned Period Percent : <xsl:value-of select="@TLPPPercent5"/>
        </div>
        <div class="ui-block-a">
          L Planned Cumul Percent : <xsl:value-of select="@TLPCPercent5"/>
        </div>
        <div class="ui-block-b">
          L Planned Full Percent : <xsl:value-of select="@TLPFPercent5"/>
        </div>
        <div class="ui-block-a">
          U Unit : <xsl:value-of select="@TME2TUUnit5"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          U Planned Period : <xsl:value-of select="@TME2TUAmount5"/>
        </div>
        <div class="ui-block-b">
          U Plan Full : <xsl:value-of select="@TUPFTotal5"/>
        </div>
        <div class="ui-block-a">
          U Plan Cumul : <xsl:value-of select="@TUPCTotal5"/>
        </div>
        <div class="ui-block-b">
          U Actual Period : <xsl:value-of select="@TUAPTotal5"/>
        </div>
        <div class="ui-block-a">
          U Actual Cumul : <xsl:value-of select="@TUACTotal5"/>
        </div>
        <div class="ui-block-b">
          U Actual Period Change : <xsl:value-of select="@TUAPChange5"/>
        </div>
        <div class="ui-block-a">
          U Actual Cumul Change : <xsl:value-of select="@TUACChange5"/>
        </div>
        <div class="ui-block-b">
          U Planned Period Percent : <xsl:value-of select="@TUPPPercent5"/>
        </div>
        <div class="ui-block-a">
          U Planned Cumul Percent : <xsl:value-of select="@TUPCPercent5"/>
        </div>
        <div class="ui-block-b">
          U Planned Full Percent : <xsl:value-of select="@TUPFPercent5"/>
        </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description5" />
	      </div>
      </div>
	</xsl:if>
  <xsl:if test="(@TME2Name6 != '' and @TME2Name6 != 'none')">
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
          Unit : <xsl:value-of select="@TME2TMUnit6"/>
        </div>
        <div class="ui-block-b">
           Type : <xsl:value-of select="@TME2Type6"/>
        </div>
        <div class="ui-block-a">
          Most Planned Period : <xsl:value-of select="@TME2TMAmount6"/>
        </div>
        <div class="ui-block-b">
          Most Plan Full : <xsl:value-of select="@TMPFTotal6"/>
        </div>
        <div class="ui-block-a">
          Most Plan Cumul : <xsl:value-of select="@TMPCTotal6"/>
        </div>
        <div class="ui-block-b">
          Most Actual Period : <xsl:value-of select="@TMAPTotal6"/>
        </div>
        <div class="ui-block-a">
          Most Actual Cumul : <xsl:value-of select="@TMACTotal6"/>
        </div>
        <div class="ui-block-b">
          Most Actual Period Change : <xsl:value-of select="@TMAPChange6"/>
        </div>
        <div class="ui-block-a">
          Most Actual Cumul Change : <xsl:value-of select="@TMACChange6"/>
        </div>
        <div class="ui-block-b">
          Most Planned Period Percent : <xsl:value-of select="@TMPPPercent6"/>
        </div>
        <div class="ui-block-a">
          Most Planned Cumul Percent : <xsl:value-of select="@TMPCPercent6"/>
        </div>
        <div class="ui-block-b">
          Most Planned Full Percent : <xsl:value-of select="@TMPFPercent6"/>
        </div>
        <div class="ui-block-a">
          L Unit : <xsl:value-of select="@TME2TLUnit6"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          L Planned Period : <xsl:value-of select="@TME2TLAmount6"/>
        </div>
        <div class="ui-block-b">
          L Plan Full : <xsl:value-of select="@TLPFTotal6"/>
        </div>
        <div class="ui-block-a">
          L Plan Cumul : <xsl:value-of select="@TLPCTotal6"/>
        </div>
        <div class="ui-block-b">
          L Actual Period : <xsl:value-of select="@TLAPTotal6"/>
        </div>
        <div class="ui-block-a">
          L Actual Cumul : <xsl:value-of select="@TLACTotal6"/>
        </div>
        <div class="ui-block-b">
          L Actual Period Change : <xsl:value-of select="@TLAPChange6"/>
        </div>
        <div class="ui-block-a">
          L Actual Cumul Change : <xsl:value-of select="@TLACChange6"/>
        </div>
        <div class="ui-block-b">
          L Planned Period Percent : <xsl:value-of select="@TLPPPercent6"/>
        </div>
        <div class="ui-block-a">
          L Planned Cumul Percent : <xsl:value-of select="@TLPCPercent6"/>
        </div>
        <div class="ui-block-b">
          L Planned Full Percent : <xsl:value-of select="@TLPFPercent6"/>
        </div>
        <div class="ui-block-a">
          U Unit : <xsl:value-of select="@TME2TUUnit6"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          U Planned Period : <xsl:value-of select="@TME2TUAmount6"/>
        </div>
        <div class="ui-block-b">
          U Plan Full : <xsl:value-of select="@TUPFTotal6"/>
        </div>
        <div class="ui-block-a">
          U Plan Cumul : <xsl:value-of select="@TUPCTotal6"/>
        </div>
        <div class="ui-block-b">
          U Actual Period : <xsl:value-of select="@TUAPTotal6"/>
        </div>
        <div class="ui-block-a">
          U Actual Cumul : <xsl:value-of select="@TUACTotal6"/>
        </div>
        <div class="ui-block-b">
          U Actual Period Change : <xsl:value-of select="@TUAPChange6"/>
        </div>
        <div class="ui-block-a">
          U Actual Cumul Change : <xsl:value-of select="@TUACChange6"/>
        </div>
        <div class="ui-block-b">
          U Planned Period Percent : <xsl:value-of select="@TUPPPercent6"/>
        </div>
        <div class="ui-block-a">
          U Planned Cumul Percent : <xsl:value-of select="@TUPCPercent6"/>
        </div>
        <div class="ui-block-b">
          U Planned Full Percent : <xsl:value-of select="@TUPFPercent6"/>
        </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description6" />
	      </div>
      </div>
	</xsl:if>
  <xsl:if test="(@TME2Name7 != '' and @TME2Name7 != 'none')">
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
          Unit : <xsl:value-of select="@TME2TMUnit7"/>
        </div>
        <div class="ui-block-b">
           Type : <xsl:value-of select="@TME2Type7"/>
        </div>
        <div class="ui-block-a">
          Most Planned Period : <xsl:value-of select="@TME2TMAmount7"/>
        </div>
        <div class="ui-block-b">
          Most Plan Full : <xsl:value-of select="@TMPFTotal7"/>
        </div>
        <div class="ui-block-a">
          Most Plan Cumul : <xsl:value-of select="@TMPCTotal7"/>
        </div>
        <div class="ui-block-b">
          Most Actual Period : <xsl:value-of select="@TMAPTotal7"/>
        </div>
        <div class="ui-block-a">
          Most Actual Cumul : <xsl:value-of select="@TMACTotal7"/>
        </div>
        <div class="ui-block-b">
          Most Actual Period Change : <xsl:value-of select="@TMAPChange7"/>
        </div>
        <div class="ui-block-a">
          Most Actual Cumul Change : <xsl:value-of select="@TMACChange7"/>
        </div>
        <div class="ui-block-b">
          Most Planned Period Percent : <xsl:value-of select="@TMPPPercent7"/>
        </div>
        <div class="ui-block-a">
          Most Planned Cumul Percent : <xsl:value-of select="@TMPCPercent7"/>
        </div>
        <div class="ui-block-b">
          Most Planned Full Percent : <xsl:value-of select="@TMPFPercent7"/>
        </div>
        <div class="ui-block-a">
          L Unit : <xsl:value-of select="@TME2TLUnit7"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          L Planned Period : <xsl:value-of select="@TME2TLAmount7"/>
        </div>
        <div class="ui-block-b">
          L Plan Full : <xsl:value-of select="@TLPFTotal7"/>
        </div>
        <div class="ui-block-a">
          L Plan Cumul : <xsl:value-of select="@TLPCTotal7"/>
        </div>
        <div class="ui-block-b">
          L Actual Period : <xsl:value-of select="@TLAPTotal7"/>
        </div>
        <div class="ui-block-a">
          L Actual Cumul : <xsl:value-of select="@TLACTotal7"/>
        </div>
        <div class="ui-block-b">
          L Actual Period Change : <xsl:value-of select="@TLAPChange7"/>
        </div>
        <div class="ui-block-a">
          L Actual Cumul Change : <xsl:value-of select="@TLACChange7"/>
        </div>
        <div class="ui-block-b">
          L Planned Period Percent : <xsl:value-of select="@TLPPPercent7"/>
        </div>
        <div class="ui-block-a">
          L Planned Cumul Percent : <xsl:value-of select="@TLPCPercent7"/>
        </div>
        <div class="ui-block-b">
          L Planned Full Percent : <xsl:value-of select="@TLPFPercent7"/>
        </div>
        <div class="ui-block-a">
          U Unit : <xsl:value-of select="@TME2TUUnit7"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          U Planned Period : <xsl:value-of select="@TME2TUAmount7"/>
        </div>
        <div class="ui-block-b">
          U Plan Full : <xsl:value-of select="@TUPFTotal7"/>
        </div>
        <div class="ui-block-a">
          U Plan Cumul : <xsl:value-of select="@TUPCTotal7"/>
        </div>
        <div class="ui-block-b">
          U Actual Period : <xsl:value-of select="@TUAPTotal7"/>
        </div>
        <div class="ui-block-a">
          U Actual Cumul : <xsl:value-of select="@TUACTotal7"/>
        </div>
        <div class="ui-block-b">
          U Actual Period Change : <xsl:value-of select="@TUAPChange7"/>
        </div>
        <div class="ui-block-a">
          U Actual Cumul Change : <xsl:value-of select="@TUACChange7"/>
        </div>
        <div class="ui-block-b">
          U Planned Period Percent : <xsl:value-of select="@TUPPPercent7"/>
        </div>
        <div class="ui-block-a">
          U Planned Cumul Percent : <xsl:value-of select="@TUPCPercent7"/>
        </div>
        <div class="ui-block-b">
          U Planned Full Percent : <xsl:value-of select="@TUPFPercent7"/>
        </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description7" />
	      </div>
      </div>
	</xsl:if>
  <xsl:if test="(@TME2Name8 != '' and @TME2Name8 != 'none')">
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
          Unit : <xsl:value-of select="@TME2TMUnit8"/>
        </div>
        <div class="ui-block-b">
           Type : <xsl:value-of select="@TME2Type8"/>
        </div>
        <div class="ui-block-a">
          Most Planned Period : <xsl:value-of select="@TME2TMAmount8"/>
        </div>
        <div class="ui-block-b">
          Most Plan Full : <xsl:value-of select="@TMPFTotal8"/>
        </div>
        <div class="ui-block-a">
          Most Plan Cumul : <xsl:value-of select="@TMPCTotal8"/>
        </div>
        <div class="ui-block-b">
          Most Actual Period : <xsl:value-of select="@TMAPTotal8"/>
        </div>
        <div class="ui-block-a">
          Most Actual Cumul : <xsl:value-of select="@TMACTotal8"/>
        </div>
        <div class="ui-block-b">
          Most Actual Period Change : <xsl:value-of select="@TMAPChange8"/>
        </div>
        <div class="ui-block-a">
          Most Actual Cumul Change : <xsl:value-of select="@TMACChange8"/>
        </div>
        <div class="ui-block-b">
          Most Planned Period Percent : <xsl:value-of select="@TMPPPercent8"/>
        </div>
        <div class="ui-block-a">
          Most Planned Cumul Percent : <xsl:value-of select="@TMPCPercent8"/>
        </div>
        <div class="ui-block-b">
          Most Planned Full Percent : <xsl:value-of select="@TMPFPercent8"/>
        </div>
        <div class="ui-block-a">
          L Unit : <xsl:value-of select="@TME2TLUnit8"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          L Planned Period : <xsl:value-of select="@TME2TLAmount8"/>
        </div>
        <div class="ui-block-b">
          L Plan Full : <xsl:value-of select="@TLPFTotal8"/>
        </div>
        <div class="ui-block-a">
          L Plan Cumul : <xsl:value-of select="@TLPCTotal8"/>
        </div>
        <div class="ui-block-b">
          L Actual Period : <xsl:value-of select="@TLAPTotal8"/>
        </div>
        <div class="ui-block-a">
          L Actual Cumul : <xsl:value-of select="@TLACTotal8"/>
        </div>
        <div class="ui-block-b">
          L Actual Period Change : <xsl:value-of select="@TLAPChange8"/>
        </div>
        <div class="ui-block-a">
          L Actual Cumul Change : <xsl:value-of select="@TLACChange8"/>
        </div>
        <div class="ui-block-b">
          L Planned Period Percent : <xsl:value-of select="@TLPPPercent8"/>
        </div>
        <div class="ui-block-a">
          L Planned Cumul Percent : <xsl:value-of select="@TLPCPercent8"/>
        </div>
        <div class="ui-block-b">
          L Planned Full Percent : <xsl:value-of select="@TLPFPercent8"/>
        </div>
        <div class="ui-block-a">
          U Unit : <xsl:value-of select="@TME2TUUnit8"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          U Planned Period : <xsl:value-of select="@TME2TUAmount8"/>
        </div>
        <div class="ui-block-b">
          U Plan Full : <xsl:value-of select="@TUPFTotal8"/>
        </div>
        <div class="ui-block-a">
          U Plan Cumul : <xsl:value-of select="@TUPCTotal8"/>
        </div>
        <div class="ui-block-b">
          U Actual Period : <xsl:value-of select="@TUAPTotal8"/>
        </div>
        <div class="ui-block-a">
          U Actual Cumul : <xsl:value-of select="@TUACTotal8"/>
        </div>
        <div class="ui-block-b">
          U Actual Period Change : <xsl:value-of select="@TUAPChange8"/>
        </div>
        <div class="ui-block-a">
          U Actual Cumul Change : <xsl:value-of select="@TUACChange8"/>
        </div>
        <div class="ui-block-b">
          U Planned Period Percent : <xsl:value-of select="@TUPPPercent8"/>
        </div>
        <div class="ui-block-a">
          U Planned Cumul Percent : <xsl:value-of select="@TUPCPercent8"/>
        </div>
        <div class="ui-block-b">
          U Planned Full Percent : <xsl:value-of select="@TUPFPercent8"/>
        </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description8" />
	      </div>
      </div>
	</xsl:if>
  <xsl:if test="(@TME2Name9 != '' and @TME2Name9 != 'none')">
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
          Unit : <xsl:value-of select="@TME2TMUnit9"/>
        </div>
        <div class="ui-block-b">
           Type : <xsl:value-of select="@TME2Type9"/>
        </div>
        <div class="ui-block-a">
          Most Planned Period : <xsl:value-of select="@TME2TMAmount9"/>
        </div>
        <div class="ui-block-b">
          Most Plan Full : <xsl:value-of select="@TMPFTotal9"/>
        </div>
        <div class="ui-block-a">
          Most Plan Cumul : <xsl:value-of select="@TMPCTotal9"/>
        </div>
        <div class="ui-block-b">
          Most Actual Period : <xsl:value-of select="@TMAPTotal9"/>
        </div>
        <div class="ui-block-a">
          Most Actual Cumul : <xsl:value-of select="@TMACTotal9"/>
        </div>
        <div class="ui-block-b">
          Most Actual Period Change : <xsl:value-of select="@TMAPChange9"/>
        </div>
        <div class="ui-block-a">
          Most Actual Cumul Change : <xsl:value-of select="@TMACChange9"/>
        </div>
        <div class="ui-block-b">
          Most Planned Period Percent : <xsl:value-of select="@TMPPPercent9"/>
        </div>
        <div class="ui-block-a">
          Most Planned Cumul Percent : <xsl:value-of select="@TMPCPercent9"/>
        </div>
        <div class="ui-block-b">
          Most Planned Full Percent : <xsl:value-of select="@TMPFPercent9"/>
        </div>
        <div class="ui-block-a">
          L Unit : <xsl:value-of select="@TME2TLUnit9"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          L Planned Period : <xsl:value-of select="@TME2TLAmount9"/>
        </div>
        <div class="ui-block-b">
          L Plan Full : <xsl:value-of select="@TLPFTotal9"/>
        </div>
        <div class="ui-block-a">
          L Plan Cumul : <xsl:value-of select="@TLPCTotal9"/>
        </div>
        <div class="ui-block-b">
          L Actual Period : <xsl:value-of select="@TLAPTotal9"/>
        </div>
        <div class="ui-block-a">
          L Actual Cumul : <xsl:value-of select="@TLACTotal9"/>
        </div>
        <div class="ui-block-b">
          L Actual Period Change : <xsl:value-of select="@TLAPChange9"/>
        </div>
        <div class="ui-block-a">
          L Actual Cumul Change : <xsl:value-of select="@TLACChange9"/>
        </div>
        <div class="ui-block-b">
          L Planned Period Percent : <xsl:value-of select="@TLPPPercent9"/>
        </div>
        <div class="ui-block-a">
          L Planned Cumul Percent : <xsl:value-of select="@TLPCPercent9"/>
        </div>
        <div class="ui-block-b">
          L Planned Full Percent : <xsl:value-of select="@TLPFPercent9"/>
        </div>
        <div class="ui-block-a">
          U Unit : <xsl:value-of select="@TME2TUUnit9"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          U Planned Period : <xsl:value-of select="@TME2TUAmount9"/>
        </div>
        <div class="ui-block-b">
          U Plan Full : <xsl:value-of select="@TUPFTotal9"/>
        </div>
        <div class="ui-block-a">
          U Plan Cumul : <xsl:value-of select="@TUPCTotal9"/>
        </div>
        <div class="ui-block-b">
          U Actual Period : <xsl:value-of select="@TUAPTotal9"/>
        </div>
        <div class="ui-block-a">
          U Actual Cumul : <xsl:value-of select="@TUACTotal9"/>
        </div>
        <div class="ui-block-b">
          U Actual Period Change : <xsl:value-of select="@TUAPChange9"/>
        </div>
        <div class="ui-block-a">
          U Actual Cumul Change : <xsl:value-of select="@TUACChange9"/>
        </div>
        <div class="ui-block-b">
          U Planned Period Percent : <xsl:value-of select="@TUPPPercent9"/>
        </div>
        <div class="ui-block-a">
          U Planned Cumul Percent : <xsl:value-of select="@TUPCPercent9"/>
        </div>
        <div class="ui-block-b">
          U Planned Full Percent : <xsl:value-of select="@TUPFPercent9"/>
        </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description9" />
	      </div>
      </div>
	</xsl:if>
  <xsl:if test="(@TME2Name10 != '' and @TME2Name10 != 'none')">
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
          Unit : <xsl:value-of select="@TME2TMUnit10"/>
        </div>
        <div class="ui-block-b">
           Type : <xsl:value-of select="@TME2Type10"/>
        </div>
        <div class="ui-block-a">
          Most Planned Period : <xsl:value-of select="@TME2TMAmount10"/>
        </div>
        <div class="ui-block-b">
          Most Plan Full : <xsl:value-of select="@TMPFTotal10"/>
        </div>
        <div class="ui-block-a">
          Most Plan Cumul : <xsl:value-of select="@TMPCTotal10"/>
        </div>
        <div class="ui-block-b">
          Most Actual Period : <xsl:value-of select="@TMAPTotal10"/>
        </div>
        <div class="ui-block-a">
          Most Actual Cumul : <xsl:value-of select="@TMACTotal10"/>
        </div>
        <div class="ui-block-b">
          Most Actual Period Change : <xsl:value-of select="@TMAPChange10"/>
        </div>
        <div class="ui-block-a">
          Most Actual Cumul Change : <xsl:value-of select="@TMACChange10"/>
        </div>
        <div class="ui-block-b">
          Most Planned Period Percent : <xsl:value-of select="@TMPPPercent10"/>
        </div>
        <div class="ui-block-a">
          Most Planned Cumul Percent : <xsl:value-of select="@TMPCPercent10"/>
        </div>
        <div class="ui-block-b">
          Most Planned Full Percent : <xsl:value-of select="@TMPFPercent10"/>
        </div>
        <div class="ui-block-a">
          L Unit : <xsl:value-of select="@TME2TLUnit10"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          L Planned Period : <xsl:value-of select="@TME2TLAmount10"/>
        </div>
        <div class="ui-block-b">
          L Plan Full : <xsl:value-of select="@TLPFTotal10"/>
        </div>
        <div class="ui-block-a">
          L Plan Cumul : <xsl:value-of select="@TLPCTotal10"/>
        </div>
        <div class="ui-block-b">
          L Actual Period : <xsl:value-of select="@TLAPTotal10"/>
        </div>
        <div class="ui-block-a">
          L Actual Cumul : <xsl:value-of select="@TLACTotal10"/>
        </div>
        <div class="ui-block-b">
          L Actual Period Change : <xsl:value-of select="@TLAPChange10"/>
        </div>
        <div class="ui-block-a">
          L Actual Cumul Change : <xsl:value-of select="@TLACChange10"/>
        </div>
        <div class="ui-block-b">
          L Planned Period Percent : <xsl:value-of select="@TLPPPercent10"/>
        </div>
        <div class="ui-block-a">
          L Planned Cumul Percent : <xsl:value-of select="@TLPCPercent10"/>
        </div>
        <div class="ui-block-b">
          L Planned Full Percent : <xsl:value-of select="@TLPFPercent10"/>
        </div>
        <div class="ui-block-a">
          U Unit : <xsl:value-of select="@TME2TUUnit10"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          U Planned Period : <xsl:value-of select="@TME2TUAmount10"/>
        </div>
        <div class="ui-block-b">
          U Plan Full : <xsl:value-of select="@TUPFTotal10"/>
        </div>
        <div class="ui-block-a">
          U Plan Cumul : <xsl:value-of select="@TUPCTotal10"/>
        </div>
        <div class="ui-block-b">
          U Actual Period : <xsl:value-of select="@TUAPTotal10"/>
        </div>
        <div class="ui-block-a">
          U Actual Cumul : <xsl:value-of select="@TUACTotal10"/>
        </div>
        <div class="ui-block-b">
          U Actual Period Change : <xsl:value-of select="@TUAPChange10"/>
        </div>
        <div class="ui-block-a">
          U Actual Cumul Change : <xsl:value-of select="@TUACChange10"/>
        </div>
        <div class="ui-block-b">
          U Planned Period Percent : <xsl:value-of select="@TUPPPercent10"/>
        </div>
        <div class="ui-block-a">
          U Planned Cumul Percent : <xsl:value-of select="@TUPCPercent10"/>
        </div>
        <div class="ui-block-b">
          U Planned Full Percent : <xsl:value-of select="@TUPFPercent10"/>
        </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description10" />
	      </div>
      </div>
	</xsl:if>
  <xsl:if test="(@TME2Name11 != '' and @TME2Name11 != 'none')">
      <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
      <h4 class="ui-bar-b">
        <strong>Indicator Details</strong>
      </h4>
      <div class="ui-grid-a">
        <div class="ui-block-a">
          Indicator 11 Name : <strong><xsl:value-of select="@TME2Name11"/></strong>
        </div>
        <div class="ui-block-b">
          Label : <xsl:value-of select="@TME2Label11"/>
        </div>
        <div class="ui-block-a">
          Date : <xsl:value-of select="@TME2Date11"/>
        </div>
        <div class="ui-block-b">
          Observations : <xsl:value-of select="@TME2N11"/>
        </div>
        <div class="ui-block-a">
          Unit : <xsl:value-of select="@TME2TMUnit11"/>
        </div>
        <div class="ui-block-b">
           Type : <xsl:value-of select="@TME2Type11"/>
        </div>
        <div class="ui-block-a">
          Most Planned Period : <xsl:value-of select="@TME2TMAmount11"/>
        </div>
        <div class="ui-block-b">
          Most Plan Full : <xsl:value-of select="@TMPFTotal11"/>
        </div>
        <div class="ui-block-a">
          Most Plan Cumul : <xsl:value-of select="@TMPCTotal11"/>
        </div>
        <div class="ui-block-b">
          Most Actual Period : <xsl:value-of select="@TMAPTotal11"/>
        </div>
        <div class="ui-block-a">
          Most Actual Cumul : <xsl:value-of select="@TMACTotal11"/>
        </div>
        <div class="ui-block-b">
          Most Actual Period Change : <xsl:value-of select="@TMAPChange11"/>
        </div>
        <div class="ui-block-a">
          Most Actual Cumul Change : <xsl:value-of select="@TMACChange11"/>
        </div>
        <div class="ui-block-b">
          Most Planned Period Percent : <xsl:value-of select="@TMPPPercent11"/>
        </div>
        <div class="ui-block-a">
          Most Planned Cumul Percent : <xsl:value-of select="@TMPCPercent11"/>
        </div>
        <div class="ui-block-b">
          Most Planned Full Percent : <xsl:value-of select="@TMPFPercent11"/>
        </div>
        <div class="ui-block-a">
          L Unit : <xsl:value-of select="@TME2TLUnit11"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          L Planned Period : <xsl:value-of select="@TME2TLAmount11"/>
        </div>
        <div class="ui-block-b">
          L Plan Full : <xsl:value-of select="@TLPFTotal11"/>
        </div>
        <div class="ui-block-a">
          L Plan Cumul : <xsl:value-of select="@TLPCTotal11"/>
        </div>
        <div class="ui-block-b">
          L Actual Period : <xsl:value-of select="@TLAPTotal11"/>
        </div>
        <div class="ui-block-a">
          L Actual Cumul : <xsl:value-of select="@TLACTotal11"/>
        </div>
        <div class="ui-block-b">
          L Actual Period Change : <xsl:value-of select="@TLAPChange11"/>
        </div>
        <div class="ui-block-a">
          L Actual Cumul Change : <xsl:value-of select="@TLACChange11"/>
        </div>
        <div class="ui-block-b">
          L Planned Period Percent : <xsl:value-of select="@TLPPPercent11"/>
        </div>
        <div class="ui-block-a">
          L Planned Cumul Percent : <xsl:value-of select="@TLPCPercent11"/>
        </div>
        <div class="ui-block-b">
          L Planned Full Percent : <xsl:value-of select="@TLPFPercent11"/>
        </div>
        <div class="ui-block-a">
          U Unit : <xsl:value-of select="@TME2TUUnit11"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          U Planned Period : <xsl:value-of select="@TME2TUAmount11"/>
        </div>
        <div class="ui-block-b">
          U Plan Full : <xsl:value-of select="@TUPFTotal11"/>
        </div>
        <div class="ui-block-a">
          U Plan Cumul : <xsl:value-of select="@TUPCTotal11"/>
        </div>
        <div class="ui-block-b">
          U Actual Period : <xsl:value-of select="@TUAPTotal11"/>
        </div>
        <div class="ui-block-a">
          U Actual Cumul : <xsl:value-of select="@TUACTotal11"/>
        </div>
        <div class="ui-block-b">
          U Actual Period Change : <xsl:value-of select="@TUAPChange11"/>
        </div>
        <div class="ui-block-a">
          U Actual Cumul Change : <xsl:value-of select="@TUACChange11"/>
        </div>
        <div class="ui-block-b">
          U Planned Period Percent : <xsl:value-of select="@TUPPPercent11"/>
        </div>
        <div class="ui-block-a">
          U Planned Cumul Percent : <xsl:value-of select="@TUPCPercent11"/>
        </div>
        <div class="ui-block-b">
          U Planned Full Percent : <xsl:value-of select="@TUPFPercent11"/>
        </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description11" />
	      </div>
      </div>
	</xsl:if>
    <xsl:if test="(@TME2Name12 != '' and @TME2Name12 != 'none')">
      <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
      <h4 class="ui-bar-b">
        <strong>Indicator Details</strong>
      </h4>
      <div class="ui-grid-a">
        <div class="ui-block-a">
          Indicator 12 Name : <strong><xsl:value-of select="@TME2Name12"/></strong>
        </div>
        <div class="ui-block-b">
          Label : <xsl:value-of select="@TME2Label12"/>
        </div>
        <div class="ui-block-a">
          Date : <xsl:value-of select="@TME2Date12"/>
        </div>
        <div class="ui-block-b">
          Observations : <xsl:value-of select="@TME2N12"/>
        </div>
        <div class="ui-block-a">
          Unit : <xsl:value-of select="@TME2TMUnit12"/>
        </div>
        <div class="ui-block-b">
           Type : <xsl:value-of select="@TME2Type12"/>
        </div>
        <div class="ui-block-a">
          Most Planned Period : <xsl:value-of select="@TME2TMAmount12"/>
        </div>
        <div class="ui-block-b">
          Most Plan Full : <xsl:value-of select="@TMPFTotal12"/>
        </div>
        <div class="ui-block-a">
          Most Plan Cumul : <xsl:value-of select="@TMPCTotal12"/>
        </div>
        <div class="ui-block-b">
          Most Actual Period : <xsl:value-of select="@TMAPTotal12"/>
        </div>
        <div class="ui-block-a">
          Most Actual Cumul : <xsl:value-of select="@TMACTotal12"/>
        </div>
        <div class="ui-block-b">
          Most Actual Period Change : <xsl:value-of select="@TMAPChange12"/>
        </div>
        <div class="ui-block-a">
          Most Actual Cumul Change : <xsl:value-of select="@TMACChange12"/>
        </div>
        <div class="ui-block-b">
          Most Planned Period Percent : <xsl:value-of select="@TMPPPercent12"/>
        </div>
        <div class="ui-block-a">
          Most Planned Cumul Percent : <xsl:value-of select="@TMPCPercent12"/>
        </div>
        <div class="ui-block-b">
          Most Planned Full Percent : <xsl:value-of select="@TMPFPercent12"/>
        </div>
        <div class="ui-block-a">
          L Unit : <xsl:value-of select="@TME2TLUnit12"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          L Planned Period : <xsl:value-of select="@TME2TLAmount12"/>
        </div>
        <div class="ui-block-b">
          L Plan Full : <xsl:value-of select="@TLPFTotal12"/>
        </div>
        <div class="ui-block-a">
          L Plan Cumul : <xsl:value-of select="@TLPCTotal12"/>
        </div>
        <div class="ui-block-b">
          L Actual Period : <xsl:value-of select="@TLAPTotal12"/>
        </div>
        <div class="ui-block-a">
          L Actual Cumul : <xsl:value-of select="@TLACTotal12"/>
        </div>
        <div class="ui-block-b">
          L Actual Period Change : <xsl:value-of select="@TLAPChange12"/>
        </div>
        <div class="ui-block-a">
          L Actual Cumul Change : <xsl:value-of select="@TLACChange12"/>
        </div>
        <div class="ui-block-b">
          L Planned Period Percent : <xsl:value-of select="@TLPPPercent12"/>
        </div>
        <div class="ui-block-a">
          L Planned Cumul Percent : <xsl:value-of select="@TLPCPercent12"/>
        </div>
        <div class="ui-block-b">
          L Planned Full Percent : <xsl:value-of select="@TLPFPercent12"/>
        </div>
        <div class="ui-block-a">
          U Unit : <xsl:value-of select="@TME2TUUnit12"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          U Planned Period : <xsl:value-of select="@TME2TUAmount12"/>
        </div>
        <div class="ui-block-b">
          U Plan Full : <xsl:value-of select="@TUPFTotal12"/>
        </div>
        <div class="ui-block-a">
          U Plan Cumul : <xsl:value-of select="@TUPCTotal12"/>
        </div>
        <div class="ui-block-b">
          U Actual Period : <xsl:value-of select="@TUAPTotal12"/>
        </div>
        <div class="ui-block-a">
          U Actual Cumul : <xsl:value-of select="@TUACTotal12"/>
        </div>
        <div class="ui-block-b">
          U Actual Period Change : <xsl:value-of select="@TUAPChange12"/>
        </div>
        <div class="ui-block-a">
          U Actual Cumul Change : <xsl:value-of select="@TUACChange12"/>
        </div>
        <div class="ui-block-b">
          U Planned Period Percent : <xsl:value-of select="@TUPPPercent12"/>
        </div>
        <div class="ui-block-a">
          U Planned Cumul Percent : <xsl:value-of select="@TUPCPercent12"/>
        </div>
        <div class="ui-block-b">
          U Planned Full Percent : <xsl:value-of select="@TUPFPercent12"/>
        </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description12" />
	      </div>
      </div>
	</xsl:if>
    <xsl:if test="(@TME2Name13 != '' and @TME2Name13 != 'none')">
      <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
      <h4 class="ui-bar-b">
        <strong>Indicator Details</strong>
      </h4>
      <div class="ui-grid-a">
        <div class="ui-block-a">
          Indicator 13 Name : <strong><xsl:value-of select="@TME2Name13"/></strong>
        </div>
        <div class="ui-block-b">
          Label : <xsl:value-of select="@TME2Label13"/>
        </div>
        <div class="ui-block-a">
          Date : <xsl:value-of select="@TME2Date13"/>
        </div>
        <div class="ui-block-b">
          Observations : <xsl:value-of select="@TME2N13"/>
        </div>
        <div class="ui-block-a">
          Unit : <xsl:value-of select="@TME2TMUnit13"/>
        </div>
        <div class="ui-block-b">
           Type : <xsl:value-of select="@TME2Type13"/>
        </div>
        <div class="ui-block-a">
          Most Planned Period : <xsl:value-of select="@TME2TMAmount13"/>
        </div>
        <div class="ui-block-b">
          Most Plan Full : <xsl:value-of select="@TMPFTotal13"/>
        </div>
        <div class="ui-block-a">
          Most Plan Cumul : <xsl:value-of select="@TMPCTotal13"/>
        </div>
        <div class="ui-block-b">
          Most Actual Period : <xsl:value-of select="@TMAPTotal13"/>
        </div>
        <div class="ui-block-a">
          Most Actual Cumul : <xsl:value-of select="@TMACTotal13"/>
        </div>
        <div class="ui-block-b">
          Most Actual Period Change : <xsl:value-of select="@TMAPChange13"/>
        </div>
        <div class="ui-block-a">
          Most Actual Cumul Change : <xsl:value-of select="@TMACChange13"/>
        </div>
        <div class="ui-block-b">
          Most Planned Period Percent : <xsl:value-of select="@TMPPPercent13"/>
        </div>
        <div class="ui-block-a">
          Most Planned Cumul Percent : <xsl:value-of select="@TMPCPercent13"/>
        </div>
        <div class="ui-block-b">
          Most Planned Full Percent : <xsl:value-of select="@TMPFPercent13"/>
        </div>
        <div class="ui-block-a">
          L Unit : <xsl:value-of select="@TME2TLUnit13"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          L Planned Period : <xsl:value-of select="@TME2TLAmount13"/>
        </div>
        <div class="ui-block-b">
          L Plan Full : <xsl:value-of select="@TLPFTotal13"/>
        </div>
        <div class="ui-block-a">
          L Plan Cumul : <xsl:value-of select="@TLPCTotal13"/>
        </div>
        <div class="ui-block-b">
          L Actual Period : <xsl:value-of select="@TLAPTotal13"/>
        </div>
        <div class="ui-block-a">
          L Actual Cumul : <xsl:value-of select="@TLACTotal13"/>
        </div>
        <div class="ui-block-b">
          L Actual Period Change : <xsl:value-of select="@TLAPChange13"/>
        </div>
        <div class="ui-block-a">
          L Actual Cumul Change : <xsl:value-of select="@TLACChange13"/>
        </div>
        <div class="ui-block-b">
          L Planned Period Percent : <xsl:value-of select="@TLPPPercent13"/>
        </div>
        <div class="ui-block-a">
          L Planned Cumul Percent : <xsl:value-of select="@TLPCPercent13"/>
        </div>
        <div class="ui-block-b">
          L Planned Full Percent : <xsl:value-of select="@TLPFPercent13"/>
        </div>
        <div class="ui-block-a">
          U Unit : <xsl:value-of select="@TME2TUUnit13"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          U Planned Period : <xsl:value-of select="@TME2TUAmount13"/>
        </div>
        <div class="ui-block-b">
          U Plan Full : <xsl:value-of select="@TUPFTotal13"/>
        </div>
        <div class="ui-block-a">
          U Plan Cumul : <xsl:value-of select="@TUPCTotal13"/>
        </div>
        <div class="ui-block-b">
          U Actual Period : <xsl:value-of select="@TUAPTotal13"/>
        </div>
        <div class="ui-block-a">
          U Actual Cumul : <xsl:value-of select="@TUACTotal13"/>
        </div>
        <div class="ui-block-b">
          U Actual Period Change : <xsl:value-of select="@TUAPChange13"/>
        </div>
        <div class="ui-block-a">
          U Actual Cumul Change : <xsl:value-of select="@TUACChange13"/>
        </div>
        <div class="ui-block-b">
          U Planned Period Percent : <xsl:value-of select="@TUPPPercent13"/>
        </div>
        <div class="ui-block-a">
          U Planned Cumul Percent : <xsl:value-of select="@TUPCPercent13"/>
        </div>
        <div class="ui-block-b">
          U Planned Full Percent : <xsl:value-of select="@TUPFPercent13"/>
        </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description13" />
	      </div>
      </div>
	</xsl:if>
    <xsl:if test="(@TME2Name14 != '' and @TME2Name14 != 'none')">
      <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
      <h4 class="ui-bar-b">
        <strong>Indicator Details</strong>
      </h4>
      <div class="ui-grid-a">
        <div class="ui-block-a">
          Indicator 14 Name : <strong><xsl:value-of select="@TME2Name14"/></strong>
        </div>
        <div class="ui-block-b">
          Label : <xsl:value-of select="@TME2Label14"/>
        </div>
        <div class="ui-block-a">
          Date : <xsl:value-of select="@TME2Date14"/>
        </div>
        <div class="ui-block-b">
          Observations : <xsl:value-of select="@TME2N14"/>
        </div>
        <div class="ui-block-a">
          Unit : <xsl:value-of select="@TME2TMUnit14"/>
        </div>
        <div class="ui-block-b">
           Type : <xsl:value-of select="@TME2Type14"/>
        </div>
        <div class="ui-block-a">
          Most Planned Period : <xsl:value-of select="@TME2TMAmount14"/>
        </div>
        <div class="ui-block-b">
          Most Plan Full : <xsl:value-of select="@TMPFTotal14"/>
        </div>
        <div class="ui-block-a">
          Most Plan Cumul : <xsl:value-of select="@TMPCTotal14"/>
        </div>
        <div class="ui-block-b">
          Most Actual Period : <xsl:value-of select="@TMAPTotal14"/>
        </div>
        <div class="ui-block-a">
          Most Actual Cumul : <xsl:value-of select="@TMACTotal14"/>
        </div>
        <div class="ui-block-b">
          Most Actual Period Change : <xsl:value-of select="@TMAPChange14"/>
        </div>
        <div class="ui-block-a">
          Most Actual Cumul Change : <xsl:value-of select="@TMACChange14"/>
        </div>
        <div class="ui-block-b">
          Most Planned Period Percent : <xsl:value-of select="@TMPPPercent14"/>
        </div>
        <div class="ui-block-a">
          Most Planned Cumul Percent : <xsl:value-of select="@TMPCPercent14"/>
        </div>
        <div class="ui-block-b">
          Most Planned Full Percent : <xsl:value-of select="@TMPFPercent14"/>
        </div>
        <div class="ui-block-a">
          L Unit : <xsl:value-of select="@TME2TLUnit14"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          L Planned Period : <xsl:value-of select="@TME2TLAmount14"/>
        </div>
        <div class="ui-block-b">
          L Plan Full : <xsl:value-of select="@TLPFTotal14"/>
        </div>
        <div class="ui-block-a">
          L Plan Cumul : <xsl:value-of select="@TLPCTotal14"/>
        </div>
        <div class="ui-block-b">
          L Actual Period : <xsl:value-of select="@TLAPTotal14"/>
        </div>
        <div class="ui-block-a">
          L Actual Cumul : <xsl:value-of select="@TLACTotal14"/>
        </div>
        <div class="ui-block-b">
          L Actual Period Change : <xsl:value-of select="@TLAPChange14"/>
        </div>
        <div class="ui-block-a">
          L Actual Cumul Change : <xsl:value-of select="@TLACChange14"/>
        </div>
        <div class="ui-block-b">
          L Planned Period Percent : <xsl:value-of select="@TLPPPercent14"/>
        </div>
        <div class="ui-block-a">
          L Planned Cumul Percent : <xsl:value-of select="@TLPCPercent14"/>
        </div>
        <div class="ui-block-b">
          L Planned Full Percent : <xsl:value-of select="@TLPFPercent14"/>
        </div>
        <div class="ui-block-a">
          U Unit : <xsl:value-of select="@TME2TUUnit14"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          U Planned Period : <xsl:value-of select="@TME2TUAmount14"/>
        </div>
        <div class="ui-block-b">
          U Plan Full : <xsl:value-of select="@TUPFTotal14"/>
        </div>
        <div class="ui-block-a">
          U Plan Cumul : <xsl:value-of select="@TUPCTotal14"/>
        </div>
        <div class="ui-block-b">
          U Actual Period : <xsl:value-of select="@TUAPTotal14"/>
        </div>
        <div class="ui-block-a">
          U Actual Cumul : <xsl:value-of select="@TUACTotal14"/>
        </div>
        <div class="ui-block-b">
          U Actual Period Change : <xsl:value-of select="@TUAPChange14"/>
        </div>
        <div class="ui-block-a">
          U Actual Cumul Change : <xsl:value-of select="@TUACChange14"/>
        </div>
        <div class="ui-block-b">
          U Planned Period Percent : <xsl:value-of select="@TUPPPercent14"/>
        </div>
        <div class="ui-block-a">
          U Planned Cumul Percent : <xsl:value-of select="@TUPCPercent14"/>
        </div>
        <div class="ui-block-b">
          U Planned Full Percent : <xsl:value-of select="@TUPFPercent14"/>
        </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description14" />
	      </div>
      </div>
	</xsl:if>
    <xsl:if test="(@TME2Name15 != '' and @TME2Name15 != 'none')">
      <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
      <h4 class="ui-bar-b">
        <strong>Indicator Details</strong>
      </h4>
      <div class="ui-grid-a">
        <div class="ui-block-a">
          Indicator 15 Name : <strong><xsl:value-of select="@TME2Name15"/></strong>
        </div>
        <div class="ui-block-b">
          Label : <xsl:value-of select="@TME2Label15"/>
        </div>
        <div class="ui-block-a">
          Date : <xsl:value-of select="@TME2Date15"/>
        </div>
        <div class="ui-block-b">
          Observations : <xsl:value-of select="@TME2N15"/>
        </div>
        <div class="ui-block-a">
          Unit : <xsl:value-of select="@TME2TMUnit15"/>
        </div>
        <div class="ui-block-b">
           Type : <xsl:value-of select="@TME2Type15"/>
        </div>
        <div class="ui-block-a">
          Most Planned Period : <xsl:value-of select="@TME2TMAmount15"/>
        </div>
        <div class="ui-block-b">
          Most Plan Full : <xsl:value-of select="@TMPFTotal15"/>
        </div>
        <div class="ui-block-a">
          Most Plan Cumul : <xsl:value-of select="@TMPCTotal15"/>
        </div>
        <div class="ui-block-b">
          Most Actual Period : <xsl:value-of select="@TMAPTotal15"/>
        </div>
        <div class="ui-block-a">
          Most Actual Cumul : <xsl:value-of select="@TMACTotal15"/>
        </div>
        <div class="ui-block-b">
          Most Actual Period Change : <xsl:value-of select="@TMAPChange15"/>
        </div>
        <div class="ui-block-a">
          Most Actual Cumul Change : <xsl:value-of select="@TMACChange15"/>
        </div>
        <div class="ui-block-b">
          Most Planned Period Percent : <xsl:value-of select="@TMPPPercent15"/>
        </div>
        <div class="ui-block-a">
          Most Planned Cumul Percent : <xsl:value-of select="@TMPCPercent15"/>
        </div>
        <div class="ui-block-b">
          Most Planned Full Percent : <xsl:value-of select="@TMPFPercent15"/>
        </div>
        <div class="ui-block-a">
          L Unit : <xsl:value-of select="@TME2TLUnit15"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          L Planned Period : <xsl:value-of select="@TME2TLAmount15"/>
        </div>
        <div class="ui-block-b">
          L Plan Full : <xsl:value-of select="@TLPFTotal15"/>
        </div>
        <div class="ui-block-a">
          L Plan Cumul : <xsl:value-of select="@TLPCTotal15"/>
        </div>
        <div class="ui-block-b">
          L Actual Period : <xsl:value-of select="@TLAPTotal15"/>
        </div>
        <div class="ui-block-a">
          L Actual Cumul : <xsl:value-of select="@TLACTotal15"/>
        </div>
        <div class="ui-block-b">
          L Actual Period Change : <xsl:value-of select="@TLAPChange15"/>
        </div>
        <div class="ui-block-a">
          L Actual Cumul Change : <xsl:value-of select="@TLACChange15"/>
        </div>
        <div class="ui-block-b">
          L Planned Period Percent : <xsl:value-of select="@TLPPPercent15"/>
        </div>
        <div class="ui-block-a">
          L Planned Cumul Percent : <xsl:value-of select="@TLPCPercent15"/>
        </div>
        <div class="ui-block-b">
          L Planned Full Percent : <xsl:value-of select="@TLPFPercent15"/>
        </div>
        <div class="ui-block-a">
          U Unit : <xsl:value-of select="@TME2TUUnit15"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          U Planned Period : <xsl:value-of select="@TME2TUAmount15"/>
        </div>
        <div class="ui-block-b">
          U Plan Full : <xsl:value-of select="@TUPFTotal15"/>
        </div>
        <div class="ui-block-a">
          U Plan Cumul : <xsl:value-of select="@TUPCTotal15"/>
        </div>
        <div class="ui-block-b">
          U Actual Period : <xsl:value-of select="@TUAPTotal15"/>
        </div>
        <div class="ui-block-a">
          U Actual Cumul : <xsl:value-of select="@TUACTotal15"/>
        </div>
        <div class="ui-block-b">
          U Actual Period Change : <xsl:value-of select="@TUAPChange15"/>
        </div>
        <div class="ui-block-a">
          U Actual Cumul Change : <xsl:value-of select="@TUACChange15"/>
        </div>
        <div class="ui-block-b">
          U Planned Period Percent : <xsl:value-of select="@TUPPPercent15"/>
        </div>
        <div class="ui-block-a">
          U Planned Cumul Percent : <xsl:value-of select="@TUPCPercent15"/>
        </div>
        <div class="ui-block-b">
          U Planned Full Percent : <xsl:value-of select="@TUPFPercent15"/>
        </div>
        </div>
        <div >
			    <strong>Description : </strong><xsl:value-of select="@TME2Description15" />
	      </div>
      </div>
	</xsl:if>
	</xsl:template>
</xsl:stylesheet>