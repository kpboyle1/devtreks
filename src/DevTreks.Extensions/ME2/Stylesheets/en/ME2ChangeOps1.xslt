<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2014, January -->
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
          Total : <xsl:value-of select="@TME2Total1"/>
        </div>
        <div class="ui-block-b">
          Total AmountChange : <xsl:value-of select="@TME2AmountChange1"/>
        </div>
        <div class="ui-block-a">
          Total PercentChange : <xsl:value-of select="@TME2PercentChange1"/>
        </div>
        <div class="ui-block-b">
          Total BaseChange : <xsl:value-of select="@TME2BaseChange1"/>
        </div>
        <div class="ui-block-a">
          Total BasePercentChange : <xsl:value-of select="@TME2BasePercentChange1"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Q1 Total : <xsl:value-of select="@TME2Q1Total1"/>
        </div>
        <div class="ui-block-b">
          Q1 AmountChange : <xsl:value-of select="@TME2Q1AmountChange1"/>
        </div>
        <div class="ui-block-a">
          Q1 PercentChange : <xsl:value-of select="@TME2Q1PercentChange1"/>
        </div>
        <div class="ui-block-b">
          Q1 BaseChange : <xsl:value-of select="@TME2Q1BaseChange1"/>
        </div>
        <div class="ui-block-a">
          Q1 BasePercentChange : <xsl:value-of select="@TME2Q1BasePercentChange1"/>
        </div>
        <div class="ui-block-b">
          Q1 Unit : <xsl:value-of select="@TME2Unit1"/>
        </div>
        <div class="ui-block-a">
          Q2 Total : <xsl:value-of select="@TME2Q2Total1"/>
        </div>
        <div class="ui-block-b">
          Q2 AmountChange : <xsl:value-of select="@TME2Q2AmountChange1"/>
        </div>
        <div class="ui-block-a">
          Q2 PercentChange : <xsl:value-of select="@TME2Q2PercentChange1"/>
        </div>
        <div class="ui-block-b">
          Q2 BaseChange : <xsl:value-of select="@TME2Q2BaseChange1"/>
        </div>
        <div class="ui-block-a">
          Q2 BasePercentChange : <xsl:value-of select="@TME2Q2BasePercentChange1"/>
        </div>
        <div class="ui-block-b">
          Q2 Unit : <xsl:value-of select="@TME2Q2Unit1"/>
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
          Total : <xsl:value-of select="@TME2Total2"/>
        </div>
        <div class="ui-block-b">
          Total AmountChange : <xsl:value-of select="@TME2AmountChange2"/>
        </div>
        <div class="ui-block-a">
          Total PercentChange : <xsl:value-of select="@TME2PercentChange2"/>
        </div>
        <div class="ui-block-b">
          Total BaseChange : <xsl:value-of select="@TME2BaseChange2"/>
        </div>
        <div class="ui-block-a">
          Total BasePercentChange : <xsl:value-of select="@TME2BasePercentChange2"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Q1 Total : <xsl:value-of select="@TME2Q1Total2"/>
        </div>
        <div class="ui-block-b">
          Q1 AmountChange : <xsl:value-of select="@TME2Q1AmountChange2"/>
        </div>
        <div class="ui-block-a">
          Q1 PercentChange : <xsl:value-of select="@TME2Q1PercentChange2"/>
        </div>
        <div class="ui-block-b">
          Q1 BaseChange : <xsl:value-of select="@TME2Q1BaseChange2"/>
        </div>
        <div class="ui-block-a">
          Q1 BasePercentChange : <xsl:value-of select="@TME2Q1BasePercentChange2"/>
        </div>
        <div class="ui-block-b">
          Q1 Unit : <xsl:value-of select="@TME2Unit2"/>
        </div>
        <div class="ui-block-a">
          Q2 Total : <xsl:value-of select="@TME2Q2Total2"/>
        </div>
        <div class="ui-block-b">
          Q2 AmountChange : <xsl:value-of select="@TME2Q2AmountChange2"/>
        </div>
        <div class="ui-block-a">
          Q2 PercentChange : <xsl:value-of select="@TME2Q2PercentChange2"/>
        </div>
        <div class="ui-block-b">
          Q2 BaseChange : <xsl:value-of select="@TME2Q2BaseChange2"/>
        </div>
        <div class="ui-block-a">
          Q2 BasePercentChange : <xsl:value-of select="@TME2Q2BasePercentChange2"/>
        </div>
        <div class="ui-block-b">
          Q2 Unit : <xsl:value-of select="@TME2Q2Unit2"/>
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
          Total : <xsl:value-of select="@TME2Total3"/>
        </div>
        <div class="ui-block-b">
          Total AmountChange : <xsl:value-of select="@TME2AmountChange3"/>
        </div>
        <div class="ui-block-a">
          Total PercentChange : <xsl:value-of select="@TME2PercentChange3"/>
        </div>
        <div class="ui-block-b">
          Total BaseChange : <xsl:value-of select="@TME2BaseChange3"/>
        </div>
        <div class="ui-block-a">
          Total BasePercentChange : <xsl:value-of select="@TME2BasePercentChange3"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Q1 Total : <xsl:value-of select="@TME2Q1Total3"/>
        </div>
        <div class="ui-block-b">
          Q1 AmountChange : <xsl:value-of select="@TME2Q1AmountChange3"/>
        </div>
        <div class="ui-block-a">
          Q1 PercentChange : <xsl:value-of select="@TME2Q1PercentChange3"/>
        </div>
        <div class="ui-block-b">
          Q1 BaseChange : <xsl:value-of select="@TME2Q1BaseChange3"/>
        </div>
        <div class="ui-block-a">
          Q1 BasePercentChange : <xsl:value-of select="@TME2Q1BasePercentChange3"/>
        </div>
        <div class="ui-block-b">
          Q1 Unit : <xsl:value-of select="@TME2Unit3"/>
        </div>
        <div class="ui-block-a">
          Q2 Total : <xsl:value-of select="@TME2Q2Total3"/>
        </div>
        <div class="ui-block-b">
          Q2 AmountChange : <xsl:value-of select="@TME2Q2AmountChange3"/>
        </div>
        <div class="ui-block-a">
          Q2 PercentChange : <xsl:value-of select="@TME2Q2PercentChange3"/>
        </div>
        <div class="ui-block-b">
          Q2 BaseChange : <xsl:value-of select="@TME2Q2BaseChange3"/>
        </div>
        <div class="ui-block-a">
          Q2 BasePercentChange : <xsl:value-of select="@TME2Q2BasePercentChange3"/>
        </div>
        <div class="ui-block-b">
          Q2 Unit : <xsl:value-of select="@TME2Q2Unit3"/>
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
          Total : <xsl:value-of select="@TME2Total4"/>
        </div>
        <div class="ui-block-b">
          Total AmountChange : <xsl:value-of select="@TME2AmountChange4"/>
        </div>
        <div class="ui-block-a">
          Total PercentChange : <xsl:value-of select="@TME2PercentChange4"/>
        </div>
        <div class="ui-block-b">
          Total BaseChange : <xsl:value-of select="@TME2BaseChange4"/>
        </div>
        <div class="ui-block-a">
          Total BasePercentChange : <xsl:value-of select="@TME2BasePercentChange4"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Q1 Total : <xsl:value-of select="@TME2Q1Total4"/>
        </div>
        <div class="ui-block-b">
          Q1 AmountChange : <xsl:value-of select="@TME2Q1AmountChange4"/>
        </div>
        <div class="ui-block-a">
          Q1 PercentChange : <xsl:value-of select="@TME2Q1PercentChange4"/>
        </div>
        <div class="ui-block-b">
          Q1 BaseChange : <xsl:value-of select="@TME2Q1BaseChange4"/>
        </div>
        <div class="ui-block-a">
          Q1 BasePercentChange : <xsl:value-of select="@TME2Q1BasePercentChange4"/>
        </div>
        <div class="ui-block-b">
          Q1 Unit : <xsl:value-of select="@TME2Unit4"/>
        </div>
        <div class="ui-block-a">
          Q2 Total : <xsl:value-of select="@TME2Q2Total4"/>
        </div>
        <div class="ui-block-b">
          Q2 AmountChange : <xsl:value-of select="@TME2Q2AmountChange4"/>
        </div>
        <div class="ui-block-a">
          Q2 PercentChange : <xsl:value-of select="@TME2Q2PercentChange4"/>
        </div>
        <div class="ui-block-b">
          Q2 BaseChange : <xsl:value-of select="@TME2Q2BaseChange4"/>
        </div>
        <div class="ui-block-a">
          Q2 BasePercentChange : <xsl:value-of select="@TME2Q2BasePercentChange4"/>
        </div>
        <div class="ui-block-b">
          Q2 Unit : <xsl:value-of select="@TME2Q2Unit4"/>
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
          Total : <xsl:value-of select="@TME2Total5"/>
        </div>
        <div class="ui-block-b">
          Total AmountChange : <xsl:value-of select="@TME2AmountChange5"/>
        </div>
        <div class="ui-block-a">
          Total PercentChange : <xsl:value-of select="@TME2PercentChange5"/>
        </div>
        <div class="ui-block-b">
          Total BaseChange : <xsl:value-of select="@TME2BaseChange5"/>
        </div>
        <div class="ui-block-a">
          Total BasePercentChange : <xsl:value-of select="@TME2BasePercentChange5"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Q1 Total : <xsl:value-of select="@TME2Q1Total5"/>
        </div>
        <div class="ui-block-b">
          Q1 AmountChange : <xsl:value-of select="@TME2Q1AmountChange5"/>
        </div>
        <div class="ui-block-a">
          Q1 PercentChange : <xsl:value-of select="@TME2Q1PercentChange5"/>
        </div>
        <div class="ui-block-b">
          Q1 BaseChange : <xsl:value-of select="@TME2Q1BaseChange5"/>
        </div>
        <div class="ui-block-a">
          Q1 BasePercentChange : <xsl:value-of select="@TME2Q1BasePercentChange5"/>
        </div>
        <div class="ui-block-b">
          Q1 Unit : <xsl:value-of select="@TME2Unit5"/>
        </div>
        <div class="ui-block-a">
          Q2 Total : <xsl:value-of select="@TME2Q2Total5"/>
        </div>
        <div class="ui-block-b">
          Q2 AmountChange : <xsl:value-of select="@TME2Q2AmountChange5"/>
        </div>
        <div class="ui-block-a">
          Q2 PercentChange : <xsl:value-of select="@TME2Q2PercentChange5"/>
        </div>
        <div class="ui-block-b">
          Q2 BaseChange : <xsl:value-of select="@TME2Q2BaseChange5"/>
        </div>
        <div class="ui-block-a">
          Q2 BasePercentChange : <xsl:value-of select="@TME2Q2BasePercentChange5"/>
        </div>
        <div class="ui-block-b">
          Q2 Unit : <xsl:value-of select="@TME2Q2Unit5"/>
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
          Total : <xsl:value-of select="@TME2Total6"/>
        </div>
        <div class="ui-block-b">
          Total AmountChange : <xsl:value-of select="@TME2AmountChange6"/>
        </div>
        <div class="ui-block-a">
          Total PercentChange : <xsl:value-of select="@TME2PercentChange6"/>
        </div>
        <div class="ui-block-b">
          Total BaseChange : <xsl:value-of select="@TME2BaseChange6"/>
        </div>
        <div class="ui-block-a">
          Total BasePercentChange : <xsl:value-of select="@TME2BasePercentChange6"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Q1 Total : <xsl:value-of select="@TME2Q1Total6"/>
        </div>
        <div class="ui-block-b">
          Q1 AmountChange : <xsl:value-of select="@TME2Q1AmountChange6"/>
        </div>
        <div class="ui-block-a">
          Q1 PercentChange : <xsl:value-of select="@TME2Q1PercentChange6"/>
        </div>
        <div class="ui-block-b">
          Q1 BaseChange : <xsl:value-of select="@TME2Q1BaseChange6"/>
        </div>
        <div class="ui-block-a">
          Q1 BasePercentChange : <xsl:value-of select="@TME2Q1BasePercentChange6"/>
        </div>
        <div class="ui-block-b">
          Q1 Unit : <xsl:value-of select="@TME2Unit6"/>
        </div>
        <div class="ui-block-a">
          Q2 Total : <xsl:value-of select="@TME2Q2Total6"/>
        </div>
        <div class="ui-block-b">
          Q2 AmountChange : <xsl:value-of select="@TME2Q2AmountChange6"/>
        </div>
        <div class="ui-block-a">
          Q2 PercentChange : <xsl:value-of select="@TME2Q2PercentChange6"/>
        </div>
        <div class="ui-block-b">
          Q2 BaseChange : <xsl:value-of select="@TME2Q2BaseChange6"/>
        </div>
        <div class="ui-block-a">
          Q2 BasePercentChange : <xsl:value-of select="@TME2Q2BasePercentChange6"/>
        </div>
        <div class="ui-block-b">
          Q2 Unit : <xsl:value-of select="@TME2Q2Unit6"/>
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
          Total : <xsl:value-of select="@TME2Total7"/>
        </div>
        <div class="ui-block-b">
          Total AmountChange : <xsl:value-of select="@TME2AmountChange7"/>
        </div>
        <div class="ui-block-a">
          Total PercentChange : <xsl:value-of select="@TME2PercentChange7"/>
        </div>
        <div class="ui-block-b">
          Total BaseChange : <xsl:value-of select="@TME2BaseChange7"/>
        </div>
        <div class="ui-block-a">
          Total BasePercentChange : <xsl:value-of select="@TME2BasePercentChange7"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Q1 Total : <xsl:value-of select="@TME2Q1Total7"/>
        </div>
        <div class="ui-block-b">
          Q1 AmountChange : <xsl:value-of select="@TME2Q1AmountChange7"/>
        </div>
        <div class="ui-block-a">
          Q1 PercentChange : <xsl:value-of select="@TME2Q1PercentChange7"/>
        </div>
        <div class="ui-block-b">
          Q1 BaseChange : <xsl:value-of select="@TME2Q1BaseChange7"/>
        </div>
        <div class="ui-block-a">
          Q1 BasePercentChange : <xsl:value-of select="@TME2Q1BasePercentChange7"/>
        </div>
        <div class="ui-block-b">
          Q1 Unit : <xsl:value-of select="@TME2Unit7"/>
        </div>
        <div class="ui-block-a">
          Q2 Total : <xsl:value-of select="@TME2Q2Total7"/>
        </div>
        <div class="ui-block-b">
          Q2 AmountChange : <xsl:value-of select="@TME2Q2AmountChange7"/>
        </div>
        <div class="ui-block-a">
          Q2 PercentChange : <xsl:value-of select="@TME2Q2PercentChange7"/>
        </div>
        <div class="ui-block-b">
          Q2 BaseChange : <xsl:value-of select="@TME2Q2BaseChange7"/>
        </div>
        <div class="ui-block-a">
          Q2 BasePercentChange : <xsl:value-of select="@TME2Q2BasePercentChange7"/>
        </div>
        <div class="ui-block-b">
          Q2 Unit : <xsl:value-of select="@TME2Q2Unit7"/>
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
          Total : <xsl:value-of select="@TME2Total8"/>
        </div>
        <div class="ui-block-b">
          Total AmountChange : <xsl:value-of select="@TME2AmountChange8"/>
        </div>
        <div class="ui-block-a">
          Total PercentChange : <xsl:value-of select="@TME2PercentChange8"/>
        </div>
        <div class="ui-block-b">
          Total BaseChange : <xsl:value-of select="@TME2BaseChange8"/>
        </div>
        <div class="ui-block-a">
          Total BasePercentChange : <xsl:value-of select="@TME2BasePercentChange8"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Q1 Total : <xsl:value-of select="@TME2Q1Total8"/>
        </div>
        <div class="ui-block-b">
          Q1 AmountChange : <xsl:value-of select="@TME2Q1AmountChange8"/>
        </div>
        <div class="ui-block-a">
          Q1 PercentChange : <xsl:value-of select="@TME2Q1PercentChange8"/>
        </div>
        <div class="ui-block-b">
          Q1 BaseChange : <xsl:value-of select="@TME2Q1BaseChange8"/>
        </div>
        <div class="ui-block-a">
          Q1 BasePercentChange : <xsl:value-of select="@TME2Q1BasePercentChange8"/>
        </div>
        <div class="ui-block-b">
          Q1 Unit : <xsl:value-of select="@TME2Unit8"/>
        </div>
        <div class="ui-block-a">
          Q2 Total : <xsl:value-of select="@TME2Q2Total8"/>
        </div>
        <div class="ui-block-b">
          Q2 AmountChange : <xsl:value-of select="@TME2Q2AmountChange8"/>
        </div>
        <div class="ui-block-a">
          Q2 PercentChange : <xsl:value-of select="@TME2Q2PercentChange8"/>
        </div>
        <div class="ui-block-b">
          Q2 BaseChange : <xsl:value-of select="@TME2Q2BaseChange8"/>
        </div>
        <div class="ui-block-a">
          Q2 BasePercentChange : <xsl:value-of select="@TME2Q2BasePercentChange8"/>
        </div>
        <div class="ui-block-b">
          Q2 Unit : <xsl:value-of select="@TME2Q2Unit8"/>
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
          Total : <xsl:value-of select="@TME2Total9"/>
        </div>
        <div class="ui-block-b">
          Total AmountChange : <xsl:value-of select="@TME2AmountChange9"/>
        </div>
        <div class="ui-block-a">
          Total PercentChange : <xsl:value-of select="@TME2PercentChange9"/>
        </div>
        <div class="ui-block-b">
          Total BaseChange : <xsl:value-of select="@TME2BaseChange9"/>
        </div>
        <div class="ui-block-a">
          Total BasePercentChange : <xsl:value-of select="@TME2BasePercentChange9"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Q1 Total : <xsl:value-of select="@TME2Q1Total9"/>
        </div>
        <div class="ui-block-b">
          Q1 AmountChange : <xsl:value-of select="@TME2Q1AmountChange9"/>
        </div>
        <div class="ui-block-a">
          Q1 PercentChange : <xsl:value-of select="@TME2Q1PercentChange9"/>
        </div>
        <div class="ui-block-b">
          Q1 BaseChange : <xsl:value-of select="@TME2Q1BaseChange9"/>
        </div>
        <div class="ui-block-a">
          Q1 BasePercentChange : <xsl:value-of select="@TME2Q1BasePercentChange9"/>
        </div>
        <div class="ui-block-b">
          Q1 Unit : <xsl:value-of select="@TME2Unit9"/>
        </div>
        <div class="ui-block-a">
          Q2 Total : <xsl:value-of select="@TME2Q2Total9"/>
        </div>
        <div class="ui-block-b">
          Q2 AmountChange : <xsl:value-of select="@TME2Q2AmountChange9"/>
        </div>
        <div class="ui-block-a">
          Q2 PercentChange : <xsl:value-of select="@TME2Q2PercentChange9"/>
        </div>
        <div class="ui-block-b">
          Q2 BaseChange : <xsl:value-of select="@TME2Q2BaseChange9"/>
        </div>
        <div class="ui-block-a">
          Q2 BasePercentChange : <xsl:value-of select="@TME2Q2BasePercentChange9"/>
        </div>
        <div class="ui-block-b">
          Q2 Unit : <xsl:value-of select="@TME2Q2Unit9"/>
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
          Total : <xsl:value-of select="@TME2Total10"/>
        </div>
        <div class="ui-block-b">
          Total AmountChange : <xsl:value-of select="@TME2AmountChange10"/>
        </div>
        <div class="ui-block-a">
          Total PercentChange : <xsl:value-of select="@TME2PercentChange10"/>
        </div>
        <div class="ui-block-b">
          Total BaseChange : <xsl:value-of select="@TME2BaseChange10"/>
        </div>
        <div class="ui-block-a">
          Total BasePercentChange : <xsl:value-of select="@TME2BasePercentChange10"/>
        </div>
        <div class="ui-block-b">
        </div>
        <div class="ui-block-a">
          Q1 Total : <xsl:value-of select="@TME2Q1Total10"/>
        </div>
        <div class="ui-block-b">
          Q1 AmountChange : <xsl:value-of select="@TME2Q1AmountChange10"/>
        </div>
        <div class="ui-block-a">
          Q1 PercentChange : <xsl:value-of select="@TME2Q1PercentChange10"/>
        </div>
        <div class="ui-block-b">
          Q1 BaseChange : <xsl:value-of select="@TME2Q1BaseChange10"/>
        </div>
        <div class="ui-block-a">
          Q1 BasePercentChange : <xsl:value-of select="@TME2Q1BasePercentChange10"/>
        </div>
        <div class="ui-block-b">
          Q1 Unit : <xsl:value-of select="@TME2Unit10"/>
        </div>
        <div class="ui-block-a">
          Q2 Total : <xsl:value-of select="@TME2Q2Total10"/>
        </div>
        <div class="ui-block-b">
          Q2 AmountChange : <xsl:value-of select="@TME2Q2AmountChange10"/>
        </div>
        <div class="ui-block-a">
          Q2 PercentChange : <xsl:value-of select="@TME2Q2PercentChange10"/>
        </div>
        <div class="ui-block-b">
          Q2 BaseChange : <xsl:value-of select="@TME2Q2BaseChange10"/>
        </div>
        <div class="ui-block-a">
          Q2 BasePercentChange : <xsl:value-of select="@TME2Q2BasePercentChange10"/>
        </div>
        <div class="ui-block-b">
          Q2 Unit : <xsl:value-of select="@TME2Q2Unit10"/>
        </div>
      </div>
      <div >
			  <strong>Description : </strong><xsl:value-of select="@TME2Description10" />
	    </div>
    </div>
	</xsl:if>
	</xsl:template>
</xsl:stylesheet>