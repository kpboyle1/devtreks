<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2016, October -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Component"
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
		<div id="modEdits_divEditsDoc">
			<xsl:apply-templates select="servicebase" />
			<xsl:apply-templates select="componentgroup" />
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
		<xsl:apply-templates select="componentgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="componentgroup">
		<h4>
      Component Group: <xsl:value-of select="@Name" /> ; <xsl:value-of select="@Num_0" />
    </h4>
    <xsl:apply-templates select="component">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="component">
    <xsl:variable name="fullcolcount" select="@Files + 1"/>
    <h4 class="ui-bar-b">
      Component Name, Date, Label
    </h4>
		<xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
		<xsl:for-each select="@*">
			<xsl:variable name="att_name" select="name()"/>
			<xsl:variable name="att_value" select="."/>
			<xsl:value-of select="DisplayComps:printValue('Name_', $att_name, $att_value)"/> 
		</xsl:for-each>
    <div class="ui-grid-a">
			<xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
    </div>
    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
		<xsl:for-each select="@*">
			<xsl:variable name="att_name" select="name()"/>
			<xsl:variable name="att_value" select="."/>
			<xsl:value-of select="DisplayComps:printValue('Date_', $att_name, $att_value)"/> 
		</xsl:for-each>
    <div class="ui-grid-a">
			<xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
    </div>
    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
		<xsl:for-each select="@*">
			<xsl:variable name="att_name" select="name()"/>
			<xsl:variable name="att_value" select="."/>
			<xsl:value-of select="DisplayComps:printValue('Num_', $att_name, $att_value)"/> 
		</xsl:for-each>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mechangeyr' 
      or @AnalyzerType='mechangeid' or @AnalyzerType='mechangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
    <xsl:variable name="fullcolcount" select="@Files + 1"/>
    <xsl:if test="(@AlternativeType != '' and @AlternativeType != 'none')">
      <div>
			  Alternative Type: <strong><xsl:value-of select="@AlternativeType"/></strong>
	    </div>
    </xsl:if>
    <xsl:if test="(@TME2Name0_0 != '' and @TME2Name0_0 != 'none')">
      <div data-role="collapsible" data-collapsed="true" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          Indicator 0 
        </h4>
        <h4 class="ui-bar-b">
          Observations 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:if test="starts-with($att_name, 'TME2N0_')">
					  <xsl:value-of select="DisplayComps:printValue('TME2N0_', $att_name, $att_value)"/> 
				  </xsl:if>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Indicator 1 Name
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Name0_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Label
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Label0_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Date
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Date0_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TMUnit0_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TMAmount0_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MAmountChange0_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MPercentChange0_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MBaseChange0_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MBasePercentChange0_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TLUnit0_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TLAmount0_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LAmountChange0_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LPercentChange0_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LBaseChange0_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LBasePercentChange0_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TUUnit0_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TUAmount0_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UAmountChange0_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UPercentChange0_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UBaseChange0_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UBasePercentChange0_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
      </div>
    </xsl:if>
    <xsl:if test="(@TME2Name1_1 != '' and @TME2Name1_1 != 'none')">
      <div data-role="collapsible" data-collapsed="true" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          Indicator 1 
        </h4>
        <h4 class="ui-bar-b">
          Observations 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:if test="starts-with($att_name, 'TME2N1_')">
					  <xsl:value-of select="DisplayComps:printValue('TME2N1_', $att_name, $att_value)"/> 
				  </xsl:if>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Indicator 1 Name
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Name1_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Label
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Label1_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Date
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Date1_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TMUnit1_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TMAmount1_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MAmountChange1_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MPercentChange1_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MBaseChange1_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MBasePercentChange1_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TLUnit1_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TLAmount1_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LAmountChange1_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LPercentChange1_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LBaseChange1_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LBasePercentChange1_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TUUnit1_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TUAmount1_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UAmountChange1_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UPercentChange1_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UBaseChange1_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UBasePercentChange1_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
      </div>
    </xsl:if>
    <xsl:if test="(@TME2Name2_2 != '' and @TME2Name2_2 != 'none')">
      <div data-role="collapsible" data-collapsed="true" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          Indicator 2 
        </h4>
        <h4 class="ui-bar-b">
          Observations 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:if test="starts-with($att_name, 'TME2N2_')">
					  <xsl:value-of select="DisplayComps:printValue('TME2N2_', $att_name, $att_value)"/> 
				  </xsl:if>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Indicator 1 Name
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Name2_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Label
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Label2_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Date
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Date2_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TMUnit2_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TMAmount2_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MAmountChange2_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MPercentChange2_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MBaseChange2_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MBasePercentChange2_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TLUnit2_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TLAmount2_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LAmountChange2_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LPercentChange2_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LBaseChange2_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LBasePercentChange2_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TUUnit2_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TUAmount2_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UAmountChange2_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UPercentChange2_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UBaseChange2_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UBasePercentChange2_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
      </div>
    </xsl:if>
    <xsl:if test="(@TME2Name3_3 != '' and @TME2Name3_3 != 'none')">
      <div data-role="collapsible" data-collapsed="true" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          Indicator 3 
        </h4>
        <h4 class="ui-bar-b">
          Observations 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:if test="starts-with($att_name, 'TME2N3_')">
					  <xsl:value-of select="DisplayComps:printValue('TME2N3_', $att_name, $att_value)"/> 
				  </xsl:if>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Indicator 1 Name
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Name3_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Label
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Label3_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Date
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Date3_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TMUnit3_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TMAmount3_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MAmountChange3_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MPercentChange3_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MBaseChange3_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MBasePercentChange3_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TLUnit3_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TLAmount3_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LAmountChange3_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LPercentChange3_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LBaseChange3_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LBasePercentChange3_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TUUnit3_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TUAmount3_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UAmountChange3_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UPercentChange3_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UBaseChange3_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UBasePercentChange3_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
      </div>
    </xsl:if>
    <xsl:if test="(@TME2Name4_4 != '' and @TME2Name4_4 != 'none')">
      <div data-role="collapsible" data-collapsed="true" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          Indicator 4 
        </h4>
        <h4 class="ui-bar-b">
          Observations 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:if test="starts-with($att_name, 'TME2N4_')">
					  <xsl:value-of select="DisplayComps:printValue('TME2N4_', $att_name, $att_value)"/> 
				  </xsl:if>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Indicator 1 Name
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Name4_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Label
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Label4_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Date
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Date4_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TMUnit4_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TMAmount4_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MAmountChange4_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MPercentChange4_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MBaseChange4_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MBasePercentChange4_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TLUnit4_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TLAmount4_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LAmountChange4_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LPercentChange4_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LBaseChange4_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LBasePercentChange4_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TUUnit4_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TUAmount4_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UAmountChange4_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UPercentChange4_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UBaseChange4_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UBasePercentChange4_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
      </div>
    </xsl:if>
    <xsl:if test="(@TME2Name5_5 != '' and @TME2Name5_5 != 'none')">
      <div data-role="collapsible" data-collapsed="true" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          Indicator 5 
        </h4>
        <h4 class="ui-bar-b">
          Observations 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:if test="starts-with($att_name, 'TME2N5_')">
					  <xsl:value-of select="DisplayComps:printValue('TME2N5_', $att_name, $att_value)"/> 
				  </xsl:if>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Indicator 1 Name
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Name5_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Label
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Label5_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Date
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Date5_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TMUnit5_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TMAmount5_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MAmountChange5_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MPercentChange5_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MBaseChange5_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MBasePercentChange5_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TLUnit5_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TLAmount5_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LAmountChange5_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LPercentChange5_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LBaseChange5_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LBasePercentChange5_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TUUnit5_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TUAmount5_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UAmountChange5_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UPercentChange5_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UBaseChange5_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UBasePercentChange5_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
      </div>
    </xsl:if>
    <xsl:if test="(@TME2Name6_6 != '' and @TME2Name6_6 != 'none')">
      <div data-role="collapsible" data-collapsed="true" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          Indicator 6 
        </h4>
        <h4 class="ui-bar-b">
          Observations 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:if test="starts-with($att_name, 'TME2N6_')">
					  <xsl:value-of select="DisplayComps:printValue('TME2N6_', $att_name, $att_value)"/> 
				  </xsl:if>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Indicator 1 Name
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Name6_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Label
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Label6_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Date
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Date6_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TMUnit6_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TMAmount6_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MAmountChange6_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MPercentChange6_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MBaseChange6_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MBasePercentChange6_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TLUnit6_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TLAmount6_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LAmountChange6_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LPercentChange6_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LBaseChange6_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LBasePercentChange6_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TUUnit6_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TUAmount6_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UAmountChange6_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UPercentChange6_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UBaseChange6_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UBasePercentChange6_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
      </div>
    </xsl:if>
    <xsl:if test="(@TME2Name7_7 != '' and @TME2Name7_7 != 'none')">
      <div data-role="collapsible" data-collapsed="true" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          Indicator 7
        </h4>
        <h4 class="ui-bar-b">
          Observations 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:if test="starts-with($att_name, 'TME2N7_')">
					  <xsl:value-of select="DisplayComps:printValue('TME2N7_', $att_name, $att_value)"/> 
				  </xsl:if>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Indicator 1 Name
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Name7_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Label
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Label7_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Date
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Date7_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TMUnit7_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TMAmount7_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MAmountChange7_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MPercentChange7_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MBaseChange7_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MBasePercentChange7_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TLUnit7_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TLAmount7_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LAmountChange7_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LPercentChange7_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LBaseChange7_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LBasePercentChange7_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TUUnit7_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TUAmount7_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UAmountChange7_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UPercentChange7_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UBaseChange7_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UBasePercentChange7_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
      </div>
    </xsl:if>
    <xsl:if test="(@TME2Name8_8 != '' and @TME2Name8_8 != 'none')">
      <div data-role="collapsible" data-collapsed="true" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          Indicator 8 
        </h4>
        <h4 class="ui-bar-b">
          Observations 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:if test="starts-with($att_name, 'TME2N8_')">
					  <xsl:value-of select="DisplayComps:printValue('TME2N8_', $att_name, $att_value)"/> 
				  </xsl:if>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Indicator 1 Name
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Name8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Label
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Label8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Date
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Date8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TMUnit8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TMAmount8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MAmountChange8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MPercentChange8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MBaseChange8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MBasePercentChange8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TLUnit8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TLAmount8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LAmountChange8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LPercentChange8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LBaseChange8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LBasePercentChange8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TUUnit8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TUAmount8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UAmountChange8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UPercentChange8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UBaseChange8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UBasePercentChange8_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
      </div>
    </xsl:if>
    <xsl:if test="(@TME2Name9_9 != '' and @TME2Name9_9 != 'none')">
      <div data-role="collapsible" data-collapsed="true" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          Indicator 9 
        </h4>
        <h4 class="ui-bar-b">
          Observations 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:if test="starts-with($att_name, 'TME2N9_')">
					  <xsl:value-of select="DisplayComps:printValue('TME2N9_', $att_name, $att_value)"/> 
				  </xsl:if>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Indicator 1 Name
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Name9_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Label
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Label9_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Date
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Date9_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TMUnit9_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TMAmount9_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MAmountChange9_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MPercentChange9_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MBaseChange9_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MBasePercentChange9_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TLUnit9_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TLAmount9_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LAmountChange9_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LPercentChange9_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LBaseChange9_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LBasePercentChange9_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TUUnit9_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TUAmount9_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UAmountChange9_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UPercentChange9_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UBaseChange9_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UBasePercentChange9_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
      </div>
    </xsl:if>
    <xsl:if test="(@TME2Name10_10 != '' and @TME2Name10_10 != 'none')">
      <div data-role="collapsible" data-collapsed="true" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          Indicator 10 
        </h4>
        <h4 class="ui-bar-b">
          Observations 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:if test="starts-with($att_name, 'TME2N10_')">
					  <xsl:value-of select="DisplayComps:printValue('TME2N10_', $att_name, $att_value)"/> 
				  </xsl:if>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Indicator 1 Name
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Name10_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Label
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Label10_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Date
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Date10_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TMUnit10_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TMAmount10_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MAmountChange10_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MPercentChange10_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MBaseChange10_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MBasePercentChange10_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TLUnit10_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TLAmount10_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LAmountChange10_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LPercentChange10_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LBaseChange10_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LBasePercentChange10_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TUUnit10_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TUAmount10_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UAmountChange10_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UPercentChange10_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UBaseChange10_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UBasePercentChange10_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
      </div>
    </xsl:if>
    <xsl:if test="(@TME2Name11_11 != '' and @TME2Name11_11 != 'none')">
      <div data-role="collapsible" data-collapsed="true" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          Indicator 11 
        </h4>
        <h4 class="ui-bar-b">
          Observations 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:if test="starts-with($att_name, 'TME2N11_')">
					  <xsl:value-of select="DisplayComps:printValue('TME2N11_', $att_name, $att_value)"/> 
				  </xsl:if>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Indicator 1 Name
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Name11_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Label
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Label11_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Date
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Date11_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TMUnit11_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TMAmount11_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MAmountChange11_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MPercentChange11_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MBaseChange11_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MBasePercentChange11_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TLUnit11_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TLAmount11_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LAmountChange11_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LPercentChange11_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LBaseChange11_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LBasePercentChange11_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TUUnit11_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TUAmount11_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UAmountChange11_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UPercentChange11_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UBaseChange11_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UBasePercentChange11_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
      </div>
    </xsl:if>
    <xsl:if test="(@TME2Name12_12 != '' and @TME2Name12_12 != 'none')">
      <div data-role="collapsible" data-collapsed="true" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          Indicator 12 
        </h4>
        <h4 class="ui-bar-b">
          Observations 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:if test="starts-with($att_name, 'TME2N12_')">
					  <xsl:value-of select="DisplayComps:printValue('TME2N12_', $att_name, $att_value)"/> 
				  </xsl:if>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Indicator 1 Name
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Name12_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Label
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Label12_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Date
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Date12_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TMUnit12_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TMAmount12_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MAmountChange12_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MPercentChange12_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MBaseChange12_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MBasePercentChange12_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TLUnit12_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TLAmount12_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LAmountChange12_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LPercentChange12_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LBaseChange12_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LBasePercentChange12_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TUUnit12_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TUAmount12_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UAmountChange12_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UPercentChange12_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UBaseChange12_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UBasePercentChange12_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
      </div>
    </xsl:if>
    <xsl:if test="(@TME2Name13_13 != '' and @TME2Name13_13 != 'none')">
      <div data-role="collapsible" data-collapsed="true" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          Indicator 13 
        </h4>
        <h4 class="ui-bar-b">
          Observations 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:if test="starts-with($att_name, 'TME2N13_')">
					  <xsl:value-of select="DisplayComps:printValue('TME2N13_', $att_name, $att_value)"/> 
				  </xsl:if>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Indicator 1 Name
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Name13_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Label
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Label13_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Date
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Date13_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TMUnit13_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TMAmount13_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MAmountChange13_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MPercentChange13_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MBaseChange13_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MBasePercentChange13_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TLUnit13_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TLAmount13_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LAmountChange13_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LPercentChange13_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LBaseChange13_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LBasePercentChange13_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TUUnit13_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TUAmount13_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UAmountChange13_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UPercentChange13_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UBaseChange13_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UBasePercentChange13_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
      </div>
    </xsl:if>
    <xsl:if test="(@TME2Name14_14 != '' and @TME2Name14_14 != 'none')">
      <div data-role="collapsible" data-collapsed="true" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          Indicator 14 
        </h4>
        <h4 class="ui-bar-b">
          Observations 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:if test="starts-with($att_name, 'TME2N14_')">
					  <xsl:value-of select="DisplayComps:printValue('TME2N14_', $att_name, $att_value)"/> 
				  </xsl:if>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Indicator 1 Name
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Name14_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Label
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Label14_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Date
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Date14_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TMUnit14_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TMAmount14_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MAmountChange14_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MPercentChange14_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MBaseChange14_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MBasePercentChange14_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TLUnit14_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TLAmount14_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LAmountChange14_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LPercentChange14_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LBaseChange14_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LBasePercentChange14_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TUUnit14_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TUAmount14_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UAmountChange14_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UPercentChange14_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UBaseChange14_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UBasePercentChange14_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
      </div>
    </xsl:if>
    <xsl:if test="(@TME2Name15_15 != '' and @TME2Name15_15 != 'none')">
      <div data-role="collapsible" data-collapsed="true" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          Indicator 15 
        </h4>
        <h4 class="ui-bar-b">
          Observations 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:if test="starts-with($att_name, 'TME2N15_')">
					  <xsl:value-of select="DisplayComps:printValue('TME2N15_', $att_name, $att_value)"/> 
				  </xsl:if>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Indicator 1 Name
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Name15_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Label
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Label15_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Date
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2Date15_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TMUnit15_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TMAmount15_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MAmountChange15_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MPercentChange15_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MBaseChange15_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Most Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2MBasePercentChange15_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TLUnit15_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TLAmount15_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LAmountChange15_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LPercentChange15_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LBaseChange15_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Lower Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2LBasePercentChange15_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Unit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TUUnit15_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2TUAmount15_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Amount Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UAmountChange15_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UPercentChange15_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Base Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UBaseChange15_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Upper Base Percent Change
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TME2UBasePercentChange15_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
      </div>
    </xsl:if>
	</xsl:template>
</xsl:stylesheet>
