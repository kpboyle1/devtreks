<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2016, October -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Operation"
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
			<xsl:apply-templates select="operationgroup" />
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
		<xsl:apply-templates select="operationgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="operationgroup">
		<h4>
      <strong>Operation Group</strong> :&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mechangeyr' 
      or @AnalyzerType='mechangeid' or @AnalyzerType='mechangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="operation">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="operation">
    <h4>
      <strong>Operation </strong> :&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mechangeyr' 
      or @AnalyzerType='mechangeid' or @AnalyzerType='mechangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
		<xsl:apply-templates select="operationinput">
			<xsl:sort select="@InputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="operationinput">
    <h4>
      <strong>Input </strong> :&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mechangeyr' 
      or @AnalyzerType='mechangeid' or @AnalyzerType='mechangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
	  <xsl:param name="localName" />
    <xsl:if test="(@AlternativeType != '' and @AlternativeType != 'none')">
      <div>
			  Alternative Type: <strong><xsl:value-of select="@AlternativeType"/></strong>
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
          Most Total : <xsl:value-of select="@TME2TMAmount0"/>
        </div>
        <div class="ui-block-b">
          Most Q Change : <xsl:value-of select="@TME2MAmountChange0"/>
        </div>
        <div class="ui-block-a">
          Most % Change : <xsl:value-of select="@TME2MPercentChange0"/>
        </div>
        <div class="ui-block-b">
          Most Base Q Change : <xsl:value-of select="@TME2MBaseChange0"/>
        </div>
        <div class="ui-block-a">
          Most Base % Change : <xsl:value-of select="@TME2MBasePercentChange0"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Lower Total : <xsl:value-of select="@TME2TLAmount0"/>
        </div>
        <div class="ui-block-b">
          L Q Change : <xsl:value-of select="@TME2LAmountChange0"/>
        </div>
        <div class="ui-block-a">
          L % Change : <xsl:value-of select="@TME2LPercentChange0"/>
        </div>
        <div class="ui-block-b">
          L Q Base Change : <xsl:value-of select="@TME2LBaseChange0"/>
        </div>
        <div class="ui-block-a">
          L % Base Change : <xsl:value-of select="@TME2LBasePercentChange0"/>
        </div>
        <div class="ui-block-b">
          Lower Unit : <xsl:value-of select="@TME2TMUnit0"/>
        </div>
        <div class="ui-block-a">
          Upper Total : <xsl:value-of select="@TME2TUAmount0"/>
        </div>
        <div class="ui-block-b">
          U Q Change : <xsl:value-of select="@TME2UAmountChange0"/>
        </div>
        <div class="ui-block-a">
          U % Change : <xsl:value-of select="@TME2UPercentChange0"/>
        </div>
        <div class="ui-block-b">
          U Q Base Change : <xsl:value-of select="@TME2UBaseChange0"/>
        </div>
        <div class="ui-block-a">
          U % Base Change : <xsl:value-of select="@TME2UBasePercentChange0"/>
        </div>
        <div class="ui-block-b">
          Upper Unit : <xsl:value-of select="@TME2TUUnit0"/>
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
          Most Total : <xsl:value-of select="@TME2TMAmount1"/>
        </div>
        <div class="ui-block-b">
          M Q Change : <xsl:value-of select="@TME2MAmountChange1"/>
        </div>
        <div class="ui-block-a">
          M % Change : <xsl:value-of select="@TME2MPercentChange1"/>
        </div>
        <div class="ui-block-b">
          M Q Base Change : <xsl:value-of select="@TME2MBaseChange1"/>
        </div>
        <div class="ui-block-a">
          M % Base Change : <xsl:value-of select="@TME2MBasePercentChange1"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Lower Total : <xsl:value-of select="@TME2TLAmount1"/>
        </div>
        <div class="ui-block-b">
          L Q Change : <xsl:value-of select="@TME2LAmountChange1"/>
        </div>
        <div class="ui-block-a">
          L % Change : <xsl:value-of select="@TME2LPercentChange1"/>
        </div>
        <div class="ui-block-b">
          L Q Base Change : <xsl:value-of select="@TME2LBaseChange1"/>
        </div>
        <div class="ui-block-a">
          L % Base Change : <xsl:value-of select="@TME2LBasePercentChange1"/>
        </div>
        <div class="ui-block-b">
          Lower Unit : <xsl:value-of select="@TME2TMUnit1"/>
        </div>
        <div class="ui-block-a">
          Upper Total : <xsl:value-of select="@TME2TUAmount1"/>
        </div>
        <div class="ui-block-b">
          U Q Change : <xsl:value-of select="@TME2UAmountChange1"/>
        </div>
        <div class="ui-block-a">
          U % Change : <xsl:value-of select="@TME2UPercentChange1"/>
        </div>
        <div class="ui-block-b">
          U Q Base Change : <xsl:value-of select="@TME2UBaseChange1"/>
        </div>
        <div class="ui-block-a">
          U % Base Change : <xsl:value-of select="@TME2UBasePercentChange1"/>
        </div>
        <div class="ui-block-b">
          Upper Unit : <xsl:value-of select="@TME2TUUnit1"/>
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
          Lower : <xsl:value-of select="@TME2TMAmount2"/>
        </div>
        <div class="ui-block-b">
          M Q Change : <xsl:value-of select="@TME2MAmountChange2"/>
        </div>
        <div class="ui-block-a">
          M % Change : <xsl:value-of select="@TME2MPercentChange2"/>
        </div>
        <div class="ui-block-b">
          M Q Base Change : <xsl:value-of select="@TME2MBaseChange2"/>
        </div>
        <div class="ui-block-a">
          M % Base Change : <xsl:value-of select="@TME2MBasePercentChange2"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Lower Total : <xsl:value-of select="@TME2TLAmount2"/>
        </div>
        <div class="ui-block-b">
          L Q Change : <xsl:value-of select="@TME2LAmountChange2"/>
        </div>
        <div class="ui-block-a">
          L % Change : <xsl:value-of select="@TME2LPercentChange2"/>
        </div>
        <div class="ui-block-b">
          L Q Base Change : <xsl:value-of select="@TME2LBaseChange2"/>
        </div>
        <div class="ui-block-a">
          L % Base Change : <xsl:value-of select="@TME2LBasePercentChange2"/>
        </div>
        <div class="ui-block-b">
          Lower Unit : <xsl:value-of select="@TME2TMUnit2"/>
        </div>
        <div class="ui-block-a">
          Upper Total : <xsl:value-of select="@TME2TUAmount2"/>
        </div>
        <div class="ui-block-b">
          U Q Change : <xsl:value-of select="@TME2UAmountChange2"/>
        </div>
        <div class="ui-block-a">
          U % Change : <xsl:value-of select="@TME2UPercentChange2"/>
        </div>
        <div class="ui-block-b">
          U Q Base Change : <xsl:value-of select="@TME2UBaseChange2"/>
        </div>
        <div class="ui-block-a">
          U % Base Change : <xsl:value-of select="@TME2UBasePercentChange2"/>
        </div>
        <div class="ui-block-b">
          Upper Unit : <xsl:value-of select="@TME2TUUnit2"/>
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
          Most Total : <xsl:value-of select="@TME2TMAmount3"/>
        </div>
        <div class="ui-block-b">
          M Q Change : <xsl:value-of select="@TME2MAmountChange3"/>
        </div>
        <div class="ui-block-a">
          M % Change : <xsl:value-of select="@TME2MPercentChange3"/>
        </div>
        <div class="ui-block-b">
          M Q Base Change : <xsl:value-of select="@TME2MBaseChange3"/>
        </div>
        <div class="ui-block-a">
          M % Base Change : <xsl:value-of select="@TME2MBasePercentChange3"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Lower Total : <xsl:value-of select="@TME2TLAmount3"/>
        </div>
        <div class="ui-block-b">
          L Q Change : <xsl:value-of select="@TME2LAmountChange3"/>
        </div>
        <div class="ui-block-a">
          L % Change : <xsl:value-of select="@TME2LPercentChange3"/>
        </div>
        <div class="ui-block-b">
          L Q Base Change : <xsl:value-of select="@TME2LBaseChange3"/>
        </div>
        <div class="ui-block-a">
          L % Base Change : <xsl:value-of select="@TME2LBasePercentChange3"/>
        </div>
        <div class="ui-block-b">
          Lower Unit : <xsl:value-of select="@TME2TMUnit3"/>
        </div>
        <div class="ui-block-a">
          Upper Total : <xsl:value-of select="@TME2TUAmount3"/>
        </div>
        <div class="ui-block-b">
          U Q Change : <xsl:value-of select="@TME2UAmountChange3"/>
        </div>
        <div class="ui-block-a">
          U % Change : <xsl:value-of select="@TME2UPercentChange3"/>
        </div>
        <div class="ui-block-b">
          U Q Base Change : <xsl:value-of select="@TME2UBaseChange3"/>
        </div>
        <div class="ui-block-a">
          U % Base Change : <xsl:value-of select="@TME2UBasePercentChange3"/>
        </div>
        <div class="ui-block-b">
          Upper Unit : <xsl:value-of select="@TME2TUUnit3"/>
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
          Most Total : <xsl:value-of select="@TME2TMAmount4"/>
        </div>
        <div class="ui-block-b">
          M Q Change : <xsl:value-of select="@TME2MAmountChange4"/>
        </div>
        <div class="ui-block-a">
          M % Change : <xsl:value-of select="@TME2MPercentChange4"/>
        </div>
        <div class="ui-block-b">
          M Q Base Change : <xsl:value-of select="@TME2MBaseChange4"/>
        </div>
        <div class="ui-block-a">
          M % Base Change : <xsl:value-of select="@TME2MBasePercentChange4"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Lower Total : <xsl:value-of select="@TME2TLAmount4"/>
        </div>
        <div class="ui-block-b">
          L Q Change : <xsl:value-of select="@TME2LAmountChange4"/>
        </div>
        <div class="ui-block-a">
          L % Change : <xsl:value-of select="@TME2LPercentChange4"/>
        </div>
        <div class="ui-block-b">
          L Q Base Change : <xsl:value-of select="@TME2LBaseChange4"/>
        </div>
        <div class="ui-block-a">
          L % Base Change : <xsl:value-of select="@TME2LBasePercentChange4"/>
        </div>
        <div class="ui-block-b">
          Lower Unit : <xsl:value-of select="@TME2TMUnit4"/>
        </div>
        <div class="ui-block-a">
          Upper Total : <xsl:value-of select="@TME2TUAmount4"/>
        </div>
        <div class="ui-block-b">
          U Q Change : <xsl:value-of select="@TME2UAmountChange4"/>
        </div>
        <div class="ui-block-a">
          U % Change : <xsl:value-of select="@TME2UPercentChange4"/>
        </div>
        <div class="ui-block-b">
          U Q Base Change : <xsl:value-of select="@TME2UBaseChange4"/>
        </div>
        <div class="ui-block-a">
          U % Base Change : <xsl:value-of select="@TME2UBasePercentChange4"/>
        </div>
        <div class="ui-block-b">
          Upper Unit : <xsl:value-of select="@TME2TUUnit4"/>
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
          Most Total : <xsl:value-of select="@TME2TMAmount5"/>
        </div>
        <div class="ui-block-b">
          M Q Change : <xsl:value-of select="@TME2MAmountChange5"/>
        </div>
        <div class="ui-block-a">
          M % Change : <xsl:value-of select="@TME2MPercentChange5"/>
        </div>
        <div class="ui-block-b">
          M Q Base Change : <xsl:value-of select="@TME2MBaseChange5"/>
        </div>
        <div class="ui-block-a">
          M % Base Change : <xsl:value-of select="@TME2MBasePercentChange5"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Lower Total : <xsl:value-of select="@TME2TLAmount5"/>
        </div>
        <div class="ui-block-b">
          L Q Change : <xsl:value-of select="@TME2LAmountChange5"/>
        </div>
        <div class="ui-block-a">
          L % Change : <xsl:value-of select="@TME2LPercentChange5"/>
        </div>
        <div class="ui-block-b">
          L Q Base Change : <xsl:value-of select="@TME2LBaseChange5"/>
        </div>
        <div class="ui-block-a">
          L % Base Change : <xsl:value-of select="@TME2LBasePercentChange5"/>
        </div>
        <div class="ui-block-b">
          Lower Unit : <xsl:value-of select="@TME2TMUnit5"/>
        </div>
        <div class="ui-block-a">
          Upper Total : <xsl:value-of select="@TME2TUAmount5"/>
        </div>
        <div class="ui-block-b">
          U Q Change : <xsl:value-of select="@TME2UAmountChange5"/>
        </div>
        <div class="ui-block-a">
          U % Change : <xsl:value-of select="@TME2UPercentChange5"/>
        </div>
        <div class="ui-block-b">
          U Q Base Change : <xsl:value-of select="@TME2UBaseChange5"/>
        </div>
        <div class="ui-block-a">
          U % Base Change : <xsl:value-of select="@TME2UBasePercentChange5"/>
        </div>
        <div class="ui-block-b">
          Upper Unit : <xsl:value-of select="@TME2TUUnit5"/>
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
          Most Total : <xsl:value-of select="@TME2TMAmount6"/>
        </div>
        <div class="ui-block-b">
          M Q Change : <xsl:value-of select="@TME2MAmountChange6"/>
        </div>
        <div class="ui-block-a">
          M % Change : <xsl:value-of select="@TME2MPercentChange6"/>
        </div>
        <div class="ui-block-b">
          M Q Base Change : <xsl:value-of select="@TME2MBaseChange6"/>
        </div>
        <div class="ui-block-a">
          M % Base Change : <xsl:value-of select="@TME2MBasePercentChange6"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Lower Total : <xsl:value-of select="@TME2TLAmount6"/>
        </div>
        <div class="ui-block-b">
          L Q Change : <xsl:value-of select="@TME2LAmountChange6"/>
        </div>
        <div class="ui-block-a">
          L % Change : <xsl:value-of select="@TME2LPercentChange6"/>
        </div>
        <div class="ui-block-b">
          L Q Base Change : <xsl:value-of select="@TME2LBaseChange6"/>
        </div>
        <div class="ui-block-a">
          L % Base Change : <xsl:value-of select="@TME2LBasePercentChange6"/>
        </div>
        <div class="ui-block-b">
          Lower Unit : <xsl:value-of select="@TME2TMUnit6"/>
        </div>
        <div class="ui-block-a">
          Upper Total : <xsl:value-of select="@TME2TUAmount6"/>
        </div>
        <div class="ui-block-b">
          U Q Change : <xsl:value-of select="@TME2UAmountChange6"/>
        </div>
        <div class="ui-block-a">
          U % Change : <xsl:value-of select="@TME2UPercentChange6"/>
        </div>
        <div class="ui-block-b">
          U Q Base Change : <xsl:value-of select="@TME2UBaseChange6"/>
        </div>
        <div class="ui-block-a">
          U % Base Change : <xsl:value-of select="@TME2UBasePercentChange6"/>
        </div>
        <div class="ui-block-b">
          Upper Unit : <xsl:value-of select="@TME2TUUnit6"/>
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
          Most Total : <xsl:value-of select="@TME2TMAmount7"/>
        </div>
        <div class="ui-block-b">
          M Q Change : <xsl:value-of select="@TME2MAmountChange7"/>
        </div>
        <div class="ui-block-a">
          M % Change : <xsl:value-of select="@TME2MPercentChange7"/>
        </div>
        <div class="ui-block-b">
          M Q Base Change : <xsl:value-of select="@TME2MBaseChange7"/>
        </div>
        <div class="ui-block-a">
          M % Base Change : <xsl:value-of select="@TME2MBasePercentChange7"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Lower Total : <xsl:value-of select="@TME2TLAmount7"/>
        </div>
        <div class="ui-block-b">
          L Q Change : <xsl:value-of select="@TME2LAmountChange7"/>
        </div>
        <div class="ui-block-a">
          L % Change : <xsl:value-of select="@TME2LPercentChange7"/>
        </div>
        <div class="ui-block-b">
          L Q Base Change : <xsl:value-of select="@TME2LBaseChange7"/>
        </div>
        <div class="ui-block-a">
          L % Base Change : <xsl:value-of select="@TME2LBasePercentChange7"/>
        </div>
        <div class="ui-block-b">
          Lower Unit : <xsl:value-of select="@TME2TMUnit7"/>
        </div>
        <div class="ui-block-a">
          Upper Total : <xsl:value-of select="@TME2TUAmount7"/>
        </div>
        <div class="ui-block-b">
          U Q Change : <xsl:value-of select="@TME2UAmountChange7"/>
        </div>
        <div class="ui-block-a">
          U % Change : <xsl:value-of select="@TME2UPercentChange7"/>
        </div>
        <div class="ui-block-b">
          U Q Base Change : <xsl:value-of select="@TME2UBaseChange7"/>
        </div>
        <div class="ui-block-a">
          U % Base Change : <xsl:value-of select="@TME2UBasePercentChange7"/>
        </div>
        <div class="ui-block-b">
          Upper Unit : <xsl:value-of select="@TME2TUUnit7"/>
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
          Most Total : <xsl:value-of select="@TME2TMAmount8"/>
        </div>
        <div class="ui-block-b">
          M Q Change : <xsl:value-of select="@TME2MAmountChange8"/>
        </div>
        <div class="ui-block-a">
          M % Change : <xsl:value-of select="@TME2MPercentChange8"/>
        </div>
        <div class="ui-block-b">
          M Q Base Change : <xsl:value-of select="@TME2MBaseChange8"/>
        </div>
        <div class="ui-block-a">
          M % Base Change : <xsl:value-of select="@TME2MBasePercentChange8"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Lower Total : <xsl:value-of select="@TME2TLAmount8"/>
        </div>
        <div class="ui-block-b">
          L Q Change : <xsl:value-of select="@TME2LAmountChange8"/>
        </div>
        <div class="ui-block-a">
          L % Change : <xsl:value-of select="@TME2LPercentChange8"/>
        </div>
        <div class="ui-block-b">
          L Q Base Change : <xsl:value-of select="@TME2LBaseChange8"/>
        </div>
        <div class="ui-block-a">
          L % Base Change : <xsl:value-of select="@TME2LBasePercentChange8"/>
        </div>
        <div class="ui-block-b">
          Lower Unit : <xsl:value-of select="@TME2TMUnit8"/>
        </div>
        <div class="ui-block-a">
          Upper Total : <xsl:value-of select="@TME2TUAmount8"/>
        </div>
        <div class="ui-block-b">
          U Q Change : <xsl:value-of select="@TME2UAmountChange8"/>
        </div>
        <div class="ui-block-a">
          U % Change : <xsl:value-of select="@TME2UPercentChange8"/>
        </div>
        <div class="ui-block-b">
          U Q Base Change : <xsl:value-of select="@TME2UBaseChange8"/>
        </div>
        <div class="ui-block-a">
          U % Base Change : <xsl:value-of select="@TME2UBasePercentChange8"/>
        </div>
        <div class="ui-block-b">
          Upper Unit : <xsl:value-of select="@TME2TUUnit8"/>
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
          Most Total : <xsl:value-of select="@TME2TMAmount9"/>
        </div>
        <div class="ui-block-b">
          M Q Change : <xsl:value-of select="@TME2MAmountChange9"/>
        </div>
        <div class="ui-block-a">
          M % Change : <xsl:value-of select="@TME2MPercentChange9"/>
        </div>
        <div class="ui-block-b">
          M Q Base Change : <xsl:value-of select="@TME2MBaseChange9"/>
        </div>
        <div class="ui-block-a">
          M % Base Change : <xsl:value-of select="@TME2MBasePercentChange9"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Lower Total : <xsl:value-of select="@TME2TLAmount9"/>
        </div>
        <div class="ui-block-b">
          L Q Change : <xsl:value-of select="@TME2LAmountChange9"/>
        </div>
        <div class="ui-block-a">
          L % Change : <xsl:value-of select="@TME2LPercentChange9"/>
        </div>
        <div class="ui-block-b">
          L Q Base Change : <xsl:value-of select="@TME2LBaseChange9"/>
        </div>
        <div class="ui-block-a">
          L % Base Change : <xsl:value-of select="@TME2LBasePercentChange9"/>
        </div>
        <div class="ui-block-b">
          Lower Unit : <xsl:value-of select="@TME2TMUnit9"/>
        </div>
        <div class="ui-block-a">
          Upper Total : <xsl:value-of select="@TME2TUAmount9"/>
        </div>
        <div class="ui-block-b">
          U Q Change : <xsl:value-of select="@TME2UAmountChange9"/>
        </div>
        <div class="ui-block-a">
          U % Change : <xsl:value-of select="@TME2UPercentChange9"/>
        </div>
        <div class="ui-block-b">
          U Q Base Change : <xsl:value-of select="@TME2UBaseChange9"/>
        </div>
        <div class="ui-block-a">
          U % Base Change : <xsl:value-of select="@TME2UBasePercentChange9"/>
        </div>
        <div class="ui-block-b">
          Upper Unit : <xsl:value-of select="@TME2TUUnit9"/>
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
          Most Total : <xsl:value-of select="@TME2TMAmount10"/>
        </div>
        <div class="ui-block-b">
          M Q Change : <xsl:value-of select="@TME2MAmountChange10"/>
        </div>
        <div class="ui-block-a">
          M % Change : <xsl:value-of select="@TME2MPercentChange10"/>
        </div>
        <div class="ui-block-b">
          M Q Base Change : <xsl:value-of select="@TME2MBaseChange10"/>
        </div>
        <div class="ui-block-a">
          M % Base Change : <xsl:value-of select="@TME2MBasePercentChange10"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Lower Total : <xsl:value-of select="@TME2TLAmount10"/>
        </div>
        <div class="ui-block-b">
          L Q Change : <xsl:value-of select="@TME2LAmountChange10"/>
        </div>
        <div class="ui-block-a">
          L % Change : <xsl:value-of select="@TME2LPercentChange10"/>
        </div>
        <div class="ui-block-b">
          L Q Base Change : <xsl:value-of select="@TME2LBaseChange10"/>
        </div>
        <div class="ui-block-a">
          L % Base Change : <xsl:value-of select="@TME2LBasePercentChange10"/>
        </div>
        <div class="ui-block-b">
          Lower Unit : <xsl:value-of select="@TME2TMUnit10"/>
        </div>
        <div class="ui-block-a">
          Upper Total : <xsl:value-of select="@TME2TUAmount10"/>
        </div>
        <div class="ui-block-b">
          U Q Change : <xsl:value-of select="@TME2UAmountChange10"/>
        </div>
        <div class="ui-block-a">
          U % Change : <xsl:value-of select="@TME2UPercentChange10"/>
        </div>
        <div class="ui-block-b">
          U Q Base Change : <xsl:value-of select="@TME2UBaseChange10"/>
        </div>
        <div class="ui-block-a">
          U % Base Change : <xsl:value-of select="@TME2UBasePercentChange10"/>
        </div>
        <div class="ui-block-b">
          Upper Unit : <xsl:value-of select="@TME2TUUnit10"/>
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
          Most Total : <xsl:value-of select="@TME2TMAmount11"/>
        </div>
        <div class="ui-block-b">
          M Q Change : <xsl:value-of select="@TME2MAmountChange11"/>
        </div>
        <div class="ui-block-a">
          M % Change : <xsl:value-of select="@TME2MPercentChange11"/>
        </div>
        <div class="ui-block-b">
          M Q Base Change : <xsl:value-of select="@TME2MBaseChange11"/>
        </div>
        <div class="ui-block-a">
          M % Base Change : <xsl:value-of select="@TME2MBasePercentChange11"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Lower Total : <xsl:value-of select="@TME2TLAmount11"/>
        </div>
        <div class="ui-block-b">
          L Q Change : <xsl:value-of select="@TME2LAmountChange11"/>
        </div>
        <div class="ui-block-a">
          L % Change : <xsl:value-of select="@TME2LPercentChange11"/>
        </div>
        <div class="ui-block-b">
          L Q Base Change : <xsl:value-of select="@TME2LBaseChange11"/>
        </div>
        <div class="ui-block-a">
          L % Base Change : <xsl:value-of select="@TME2LBasePercentChange11"/>
        </div>
        <div class="ui-block-b">
          Lower Unit : <xsl:value-of select="@TME2TMUnit11"/>
        </div>
        <div class="ui-block-a">
          Upper Total : <xsl:value-of select="@TME2TUAmount11"/>
        </div>
        <div class="ui-block-b">
          U Q Change : <xsl:value-of select="@TME2UAmountChange11"/>
        </div>
        <div class="ui-block-a">
          U % Change : <xsl:value-of select="@TME2UPercentChange11"/>
        </div>
        <div class="ui-block-b">
          U Q Base Change : <xsl:value-of select="@TME2UBaseChange11"/>
        </div>
        <div class="ui-block-a">
          U % Base Change : <xsl:value-of select="@TME2UBasePercentChange11"/>
        </div>
        <div class="ui-block-b">
          Upper Unit : <xsl:value-of select="@TME2TUUnit11"/>
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
          Most Total : <xsl:value-of select="@TME2TMAmount12"/>
        </div>
        <div class="ui-block-b">
          M Q Change : <xsl:value-of select="@TME2MAmountChange12"/>
        </div>
        <div class="ui-block-a">
          M % Change : <xsl:value-of select="@TME2MPercentChange12"/>
        </div>
        <div class="ui-block-b">
          M Q Base Change : <xsl:value-of select="@TME2MBaseChange12"/>
        </div>
        <div class="ui-block-a">
          M % Base Change : <xsl:value-of select="@TME2MBasePercentChange12"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Lower Total : <xsl:value-of select="@TME2TLAmount12"/>
        </div>
        <div class="ui-block-b">
          L Q Change : <xsl:value-of select="@TME2LAmountChange12"/>
        </div>
        <div class="ui-block-a">
          L % Change : <xsl:value-of select="@TME2LPercentChange12"/>
        </div>
        <div class="ui-block-b">
          L Q Base Change : <xsl:value-of select="@TME2LBaseChange12"/>
        </div>
        <div class="ui-block-a">
          L % Base Change : <xsl:value-of select="@TME2LBasePercentChange12"/>
        </div>
        <div class="ui-block-b">
          Lower Unit : <xsl:value-of select="@TME2TMUnit12"/>
        </div>
        <div class="ui-block-a">
          Upper Total : <xsl:value-of select="@TME2TUAmount12"/>
        </div>
        <div class="ui-block-b">
          U Q Change : <xsl:value-of select="@TME2UAmountChange12"/>
        </div>
        <div class="ui-block-a">
          U % Change : <xsl:value-of select="@TME2UPercentChange12"/>
        </div>
        <div class="ui-block-b">
          U Q Base Change : <xsl:value-of select="@TME2UBaseChange12"/>
        </div>
        <div class="ui-block-a">
          U % Base Change : <xsl:value-of select="@TME2UBasePercentChange12"/>
        </div>
        <div class="ui-block-b">
          Upper Unit : <xsl:value-of select="@TME2TUUnit12"/>
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
          Most Total : <xsl:value-of select="@TME2TMAmount13"/>
        </div>
        <div class="ui-block-b">
          M Q Change : <xsl:value-of select="@TME2MAmountChange13"/>
        </div>
        <div class="ui-block-a">
          M % Change : <xsl:value-of select="@TME2MPercentChange13"/>
        </div>
        <div class="ui-block-b">
          M Q Base Change : <xsl:value-of select="@TME2MBaseChange13"/>
        </div>
        <div class="ui-block-a">
          M % Base Change : <xsl:value-of select="@TME2MBasePercentChange13"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Lower Total : <xsl:value-of select="@TME2TLAmount13"/>
        </div>
        <div class="ui-block-b">
          L Q Change : <xsl:value-of select="@TME2LAmountChange13"/>
        </div>
        <div class="ui-block-a">
          L % Change : <xsl:value-of select="@TME2LPercentChange13"/>
        </div>
        <div class="ui-block-b">
          L Q Base Change : <xsl:value-of select="@TME2LBaseChange13"/>
        </div>
        <div class="ui-block-a">
          L % Base Change : <xsl:value-of select="@TME2LBasePercentChange13"/>
        </div>
        <div class="ui-block-b">
          Lower Unit : <xsl:value-of select="@TME2TMUnit13"/>
        </div>
        <div class="ui-block-a">
          Upper Total : <xsl:value-of select="@TME2TUAmount13"/>
        </div>
        <div class="ui-block-b">
          U Q Change : <xsl:value-of select="@TME2UAmountChange13"/>
        </div>
        <div class="ui-block-a">
          U % Change : <xsl:value-of select="@TME2UPercentChange13"/>
        </div>
        <div class="ui-block-b">
          U Q Base Change : <xsl:value-of select="@TME2UBaseChange13"/>
        </div>
        <div class="ui-block-a">
          U % Base Change : <xsl:value-of select="@TME2UBasePercentChange13"/>
        </div>
        <div class="ui-block-b">
          Upper Unit : <xsl:value-of select="@TME2TUUnit13"/>
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
          Most Total : <xsl:value-of select="@TME2TMAmount14"/>
        </div>
        <div class="ui-block-b">
          M Q Change : <xsl:value-of select="@TME2MAmountChange14"/>
        </div>
        <div class="ui-block-a">
          M % Change : <xsl:value-of select="@TME2MPercentChange14"/>
        </div>
        <div class="ui-block-b">
          M Q Base Change : <xsl:value-of select="@TME2MBaseChange14"/>
        </div>
        <div class="ui-block-a">
          M % Base Change : <xsl:value-of select="@TME2MBasePercentChange14"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Lower Total : <xsl:value-of select="@TME2TLAmount14"/>
        </div>
        <div class="ui-block-b">
          L Q Change : <xsl:value-of select="@TME2LAmountChange14"/>
        </div>
        <div class="ui-block-a">
          L % Change : <xsl:value-of select="@TME2LPercentChange14"/>
        </div>
        <div class="ui-block-b">
          L Q Base Change : <xsl:value-of select="@TME2LBaseChange14"/>
        </div>
        <div class="ui-block-a">
          L % Base Change : <xsl:value-of select="@TME2LBasePercentChange14"/>
        </div>
        <div class="ui-block-b">
          Lower Unit : <xsl:value-of select="@TME2TMUnit14"/>
        </div>
        <div class="ui-block-a">
          Upper Total : <xsl:value-of select="@TME2TUAmount14"/>
        </div>
        <div class="ui-block-b">
          U Q Change : <xsl:value-of select="@TME2UAmountChange14"/>
        </div>
        <div class="ui-block-a">
          U % Change : <xsl:value-of select="@TME2UPercentChange14"/>
        </div>
        <div class="ui-block-b">
          U Q Base Change : <xsl:value-of select="@TME2UBaseChange14"/>
        </div>
        <div class="ui-block-a">
          U % Base Change : <xsl:value-of select="@TME2UBasePercentChange14"/>
        </div>
        <div class="ui-block-b">
          Upper Unit : <xsl:value-of select="@TME2TUUnit14"/>
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
          Most Total : <xsl:value-of select="@TME2TMAmount15"/>
        </div>
        <div class="ui-block-b">
          M Q Change : <xsl:value-of select="@TME2MAmountChange15"/>
        </div>
        <div class="ui-block-a">
          M % Change : <xsl:value-of select="@TME2MPercentChange15"/>
        </div>
        <div class="ui-block-b">
          M Q Base Change : <xsl:value-of select="@TME2MBaseChange15"/>
        </div>
        <div class="ui-block-a">
          M % Base Change : <xsl:value-of select="@TME2MBasePercentChange15"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Lower Total : <xsl:value-of select="@TME2TLAmount15"/>
        </div>
        <div class="ui-block-b">
          L Q Change : <xsl:value-of select="@TME2LAmountChange15"/>
        </div>
        <div class="ui-block-a">
          L % Change : <xsl:value-of select="@TME2LPercentChange15"/>
        </div>
        <div class="ui-block-b">
          L Q Base Change : <xsl:value-of select="@TME2LBaseChange15"/>
        </div>
        <div class="ui-block-a">
          L % Base Change : <xsl:value-of select="@TME2LBasePercentChange15"/>
        </div>
        <div class="ui-block-b">
          Lower Unit : <xsl:value-of select="@TME2TMUnit15"/>
        </div>
        <div class="ui-block-a">
          Upper Total : <xsl:value-of select="@TME2TUAmount15"/>
        </div>
        <div class="ui-block-b">
          U Q Change : <xsl:value-of select="@TME2UAmountChange15"/>
        </div>
        <div class="ui-block-a">
          U % Change : <xsl:value-of select="@TME2UPercentChange15"/>
        </div>
        <div class="ui-block-b">
          U Q Base Change : <xsl:value-of select="@TME2UBaseChange15"/>
        </div>
        <div class="ui-block-a">
          U % Base Change : <xsl:value-of select="@TME2UBasePercentChange15"/>
        </div>
        <div class="ui-block-b">
          Upper Unit : <xsl:value-of select="@TME2TUUnit15"/>
        </div>
      </div>
      <div >
			  <strong>Description : </strong><xsl:value-of select="@TME2Description15" />
	    </div>
    </div>
	</xsl:if>
	</xsl:template>
</xsl:stylesheet>