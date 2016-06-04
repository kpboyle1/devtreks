<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2013, October -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Investment"
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
      <strong>Investment Group</strong> : <xsl:value-of select="@Name" /> ; <xsl:value-of select="@Date_0" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcachangeyr' 
      or @AnalyzerType='lcachangeid' or @AnalyzerType='lcachangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="investment">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investment">
		<xsl:variable name="fullcolcount" select="@Files + 1"/>
    <h4 class="ui-bar-b">
      Investment Name and Date
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
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcachangeyr' 
      or @AnalyzerType='lcachangeid' or @AnalyzerType='lcachangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="investmenttimeperiod">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmenttimeperiod">
    <xsl:variable name="fullcolcount" select="@Files + 1"/>
    <h4 class="ui-bar-b">
      Time Period Name and Date
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
    <xsl:apply-templates select="root/linkedview[@AnalyzerType='lcachangeyr' 
      or @AnalyzerType='lcachangeid' or @AnalyzerType='lcachangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <div>
      <strong>Outcomes</strong>
    </div>
		<xsl:apply-templates select="investmentoutcomes" />
    <div>
      <strong>Components</strong>
    </div>
		<xsl:apply-templates select="investmentcomponents" />
	</xsl:template>
	<xsl:template match="investmentoutcomes">
		<xsl:apply-templates select="investmentoutcome">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentoutcome">
		<xsl:variable name="fullcolcount" select="@Files + 1"/>
    <h4 class="ui-bar-b">
      Outcome Name and Date
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
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcachangeyr' 
      or @AnalyzerType='lcachangeid' or @AnalyzerType='lcachangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentcomponents">
		<xsl:apply-templates select="investmentcomponent">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentcomponent">
		<xsl:variable name="fullcolcount" select="@Files + 1"/>
    <h4 class="ui-bar-b">
      Component Name and Date
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
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcachangeyr' 
      or @AnalyzerType='lcachangeid' or @AnalyzerType='lcachangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
    <xsl:variable name="fullcolcount" select="@Files + 1"/>
   <xsl:if test="($localName != 'investmentcomponent' and $localName != 'investmentinput')">
      <div data-role="collapsible" data-collapsed="true" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          Benefits
        </h4>
        <h4 class="ui-bar-b">
          Benefit Observations 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:if test="starts-with($att_name, 'Observations_')">
					  <xsl:value-of select="DisplayComps:printValue('Observations_', $att_name, $att_value)"/> 
				  </xsl:if>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Total Benefit
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TRBenefit_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Benefit AmountChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TRAmountChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Benefit PercentChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TRPercentChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Benefit BaseChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TRBaseChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Benefit BasePercentChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TRBasePercentChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          LCB Total 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TLCBBenefit_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          LCB AmountChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TLCBAmountChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          LCB PercentChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TLCBPercentChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          LCB BaseChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TLCBBaseChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          LCB BasePercentChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TLCBBasePercentChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          REAA Total 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TREAABenefit_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          REAA AmountChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TREAAAmountChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          REAA PercentChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TREAAPercentChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          REAA BaseChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TREAABaseChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          REAA BasePercentChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TREAABasePercentChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          RUnit Total 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TRUnitBenefit_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          RUnit AmountChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TRUnitAmountChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          RUnit PercentChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TRUnitPercentChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          RUnit BaseChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TRUnitBaseChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          RUnit BasePercentChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TRUnitBasePercentChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
      </div>
      <div data-role="collapsible" data-collapsed="true" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>SubBenefits</strong>
        </h4>
        <xsl:if test="(@TSubP2Name1_0 != '' and @TSubP2Name1_0 != 'none') or (@TSubP2Name1_1 != '' and @TSubP2Name1_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 1 Name 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Name1_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Amount1_0 != '' and @TSubP2Amount1_0 != 'none') or (@TSubP2Amount1_1 != '' and @TSubP2Amount1_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 1 Amount 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Amount1_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Unit1_0 != '' and @TSubP2Unit1_0 != 'none') or (@TSubP2Unit1_1 != '' and @TSubP2Unit1_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 1 Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Unit1_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Price1_0 != '' and @TSubP2Price1_0 != 'none') or (@TSubP2Price1_1 != '' and @TSubP2Price1_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 1 Price 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Price1_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Total1_0 != '' and @TSubP2Total1_0 != 'none') or (@TSubP2Total1_1 != '' and @TSubP2Total1_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 1 Total 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Total1_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2TotalPerUnit1_0 != '' and @TSubP2TotalPerUnit1_0 != 'none') or (@TSubP2TotalPerUnit1_1 != '' and @TSubP2TotalPerUnit1_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 1 Total Per Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2TotalPerUnit1_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Name2_0 != '' and @TSubP2Name2_0 != 'none') or (@TSubP2Name2_1 != '' and @TSubP2Name2_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 2 Name 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Name2_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Amount2_0 != '' and @TSubP2Amount2_0 != 'none') or (@TSubP2Amount2_1 != '' and @TSubP2Amount2_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 2 Amount 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Amount2_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Unit2_0 != '' and @TSubP2Unit2_0 != 'none') or (@TSubP2Unit2_1 != '' and @TSubP2Unit2_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 2 Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Unit2_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Price2_0 != '' and @TSubP2Price2_0 != 'none') or (@TSubP2Price2_1 != '' and @TSubP2Price2_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 2 Price 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Price2_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Total2_0 != '' and @TSubP2Total2_0 != 'none') or (@TSubP2Total2_1 != '' and @TSubP2Total2_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 2 Total 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Total2_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2TotalPerUnit2_0 != '' and @TSubP2TotalPerUnit2_0 != 'none') or (@TSubP2TotalPerUnit2_1 != '' and @TSubP2TotalPerUnit2_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 2 Total Per Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2TotalPerUnit2_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Name3_0 != '' and @TSubP2Name3_0 != 'none') or (@TSubP2Name3_1 != '' and @TSubP2Name3_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 3 Name 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Name3_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Amount3_0 != '' and @TSubP2Amount3_0 != 'none') or (@TSubP2Amount3_1 != '' and @TSubP2Amount3_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 3 Amount 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Amount3_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Unit3_0 != '' and @TSubP2Unit3_0 != 'none') or (@TSubP2Unit3_1 != '' and @TSubP2Unit3_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 3 Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Unit3_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Price3_0 != '' and @TSubP2Price3_0 != 'none') or (@TSubP2Price3_1 != '' and @TSubP2Price3_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 3 Price 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Price3_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Total3_0 != '' and @TSubP2Total3_0 != 'none') or (@TSubP2Total3_1 != '' and @TSubP2Total3_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 3 Total 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Total3_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2TotalPerUnit3_0 != '' and @TSubP2TotalPerUnit3_0 != 'none') or (@TSubP2TotalPerUnit3_1 != '' and @TSubP2TotalPerUnit3_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 3 Total Per Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2TotalPerUnit3_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Name4_0 != '' and @TSubP2Name4_0 != 'none') or (@TSubP2Name4_1 != '' and @TSubP2Name4_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 4 Name 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Name4_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Amount4_0 != '' and @TSubP2Amount4_0 != 'none') or (@TSubP2Amount4_1 != '' and @TSubP2Amount4_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 4 Amount 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Amount4_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Unit4_0 != '' and @TSubP2Unit4_0 != 'none') or (@TSubP2Unit4_1 != '' and @TSubP2Unit4_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 4 Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Unit4_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Price4_0 != '' and @TSubP2Price4_0 != 'none') or (@TSubP2Price4_1 != '' and @TSubP2Price4_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 4 Price 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Price4_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Total4_0 != '' and @TSubP2Total4_0 != 'none') or (@TSubP2Total4_1 != '' and @TSubP2Total4_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 4 Total 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Total4_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2TotalPerUnit4_0 != '' and @TSubP2TotalPerUnit4_0 != 'none') or (@TSubP2TotalPerUnit4_1 != '' and @TSubP2TotalPerUnit4_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 4 Total Per Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2TotalPerUnit4_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Name5_0 != '' and @TSubP2Name5_0 != 'none') or (@TSubP2Name5_1 != '' and @TSubP2Name5_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 5 Name 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Name5_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Amount5_0 != '' and @TSubP2Amount5_0 != 'none') or (@TSubP2Amount5_1 != '' and @TSubP2Amount5_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 5 Amount 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Amount5_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Unit5_0 != '' and @TSubP2Unit5_0 != 'none') or (@TSubP2Unit5_1 != '' and @TSubP2Unit5_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 5 Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Unit5_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Price5_0 != '' and @TSubP2Price5_0 != 'none') or (@TSubP2Price5_1 != '' and @TSubP2Price5_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 5 Price 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Price5_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Total5_0 != '' and @TSubP2Total5_0 != 'none') or (@TSubP2Total5_1 != '' and @TSubP2Total5_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 5 Total 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Total5_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2TotalPerUnit5_0 != '' and @TSubP2TotalPerUnit5_0 != 'none') or (@TSubP2TotalPerUnit5_1 != '' and @TSubP2TotalPerUnit5_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 5 Total Per Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2TotalPerUnit5_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Name6_0 != '' and @TSubP2Name6_0 != 'none') or (@TSubP2Name6_1 != '' and @TSubP2Name6_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 6 Name 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Name6_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Amount6_0 != '' and @TSubP2Amount6_0 != 'none') or (@TSubP2Amount6_1 != '' and @TSubP2Amount6_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 6 Amount 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Amount6_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Unit6_0 != '' and @TSubP2Unit6_0 != 'none') or (@TSubP2Unit6_1 != '' and @TSubP2Unit6_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 6 Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Unit6_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Price6_0 != '' and @TSubP2Price6_0 != 'none') or (@TSubP2Price6_1 != '' and @TSubP2Price6_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 6 Price 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Price6_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Total6_0 != '' and @TSubP2Total6_0 != 'none') or (@TSubP2Total6_1 != '' and @TSubP2Total6_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 6 Total 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Total6_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2TotalPerUnit6_0 != '' and @TSubP2TotalPerUnit6_0 != 'none') or (@TSubP2TotalPerUnit6_1 != '' and @TSubP2TotalPerUnit6_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 6 Total Per Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2TotalPerUnit6_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Name7_0 != '' and @TSubP2Name7_0 != 'none') or (@TSubP2Name7_1 != '' and @TSubP2Name7_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 7 Name 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Name7_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Amount7_0 != '' and @TSubP2Amount7_0 != 'none') or (@TSubP2Amount7_1 != '' and @TSubP2Amount7_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 7 Amount 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Amount7_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Unit7_0 != '' and @TSubP2Unit7_0 != 'none') or (@TSubP2Unit7_1 != '' and @TSubP2Unit7_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 7 Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Unit7_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Price7_0 != '' and @TSubP2Price7_0 != 'none') or (@TSubP2Price7_1 != '' and @TSubP2Price7_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 7 Price 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Price7_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Total7_0 != '' and @TSubP2Total7_0 != 'none') or (@TSubP2Total7_1 != '' and @TSubP2Total7_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 7 Total 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Total7_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2TotalPerUnit7_0 != '' and @TSubP2TotalPerUnit7_0 != 'none') or (@TSubP2TotalPerUnit7_1 != '' and @TSubP2TotalPerUnit7_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 7 Total Per Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2TotalPerUnit8_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Name8_0 != '' and @TSubP2Name8_0 != 'none') or (@TSubP2Name8_1 != '' and @TSubP2Name8_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 8 Name 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Name8_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Amount8_0 != '' and @TSubP2Amount8_0 != 'none') or (@TSubP2Amount8_1 != '' and @TSubP2Amount8_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 8 Amount 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Amount8_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Unit8_0 != '' and @TSubP2Unit8_0 != 'none') or (@TSubP2Unit8_1 != '' and @TSubP2Unit8_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 8 Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Unit8_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Price8_0 != '' and @TSubP2Price8_0 != 'none') or (@TSubP2Price8_1 != '' and @TSubP2Price8_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 8 Price 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Price8_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Total8_0 != '' and @TSubP2Total8_0 != 'none') or (@TSubP2Total8_1 != '' and @TSubP2Total8_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 8 Total 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Total8_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2TotalPerUnit8_0 != '' and @TSubP2TotalPerUnit8_0 != 'none') or (@TSubP2TotalPerUnit8_1 != '' and @TSubP2TotalPerUnit8_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 8 Total Per Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2TotalPerUnit8_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Name9_0 != '' and @TSubP2Name9_0 != 'none') or (@TSubP2Name9_1 != '' and @TSubP2Name9_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 9 Name 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Name9_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Amount9_0 != '' and @TSubP2Amount9_0 != 'none') or (@TSubP2Amount9_1 != '' and @TSubP2Amount9_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 9 Amount 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Amount9_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Unit9_0 != '' and @TSubP2Unit9_0 != 'none') or (@TSubP2Unit9_1 != '' and @TSubP2Unit9_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 9 Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Unit9_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Price9_0 != '' and @TSubP2Price9_0 != 'none') or (@TSubP2Price9_1 != '' and @TSubP2Price9_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 9 Price 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Price9_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Total9_0 != '' and @TSubP2Total9_0 != 'none') or (@TSubP2Total9_1 != '' and @TSubP2Total9_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 9 Total 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Total9_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2TotalPerUnit9_0 != '' and @TSubP2TotalPerUnit9_0 != 'none') or (@TSubP2TotalPerUnit9_1 != '' and @TSubP2TotalPerUnit9_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 9 Total Per Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2TotalPerUnit9_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Name10_0 != '' and @TSubP2Name10_0 != 'none') or (@TSubP2Name10_1 != '' and @TSubP2Name10_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 10 Name 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Name10_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Amount10_0 != '' and @TSubP2Amount10_0 != 'none') or (@TSubP2Amount10_1 != '' and @TSubP2Amount10_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 10 Amount 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Amount10_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Unit10_0 != '' and @TSubP2Unit10_0 != 'none') or (@TSubP2Unit10_1 != '' and @TSubP2Unit10_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 10 Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Unit10_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Price10_0 != '' and @TSubP2Price10_0 != 'none') or (@TSubP2Price10_1 != '' and @TSubP2Price10_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 10 Price 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Price10_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2Total10_0 != '' and @TSubP2Total10_0 != 'none') or (@TSubP2Total10_1 != '' and @TSubP2Total10_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 10 Total 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2Total10_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP2TotalPerUnit10_0 != '' and @TSubP2TotalPerUnit10_0 != 'none') or (@TSubP2TotalPerUnit10_1 != '' and @TSubP2TotalPerUnit10_1 != 'none')">
          <h4 class="ui-bar-b">
            SubBenefit 10 Total Per Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP2TotalPerUnit10_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
      </div>
    </xsl:if>
    <xsl:if test="($localName != 'investmentoutcome' and $localName != 'investmentoutput')">
      <div data-role="collapsible" data-collapsed="true" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          Costs
        </h4>
        <h4 class="ui-bar-b">
          Cost Observations 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:if test="starts-with($att_name, 'Observations_')">
					  <xsl:value-of select="DisplayComps:printValue('Observations_', $att_name, $att_value)"/> 
				  </xsl:if>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          OC Total 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TOCCost_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          OC AmountChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TOCAmountChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          OC PercentChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TOCPercentChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          OC BaseChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TOCBaseChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          OC BasePercentChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TOCBasePercentChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          AOH Total
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TAOHCost_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          AOH AmountChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TAOHAmountChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          AOH PercentChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TAOHPercentChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          AOH BaseChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TAOHBaseChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          AOH BasePercentChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TAOHBasePercentChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          CAP Total 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TCAPCost_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          CAP AmountChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TCAPAmountChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          CAP PercentChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TCAPPercentChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          CAP BaseChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TCAPBaseChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          CAP BasePercentChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TCAPBasePercentChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          LCC Total 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TLCCCost_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          LCC AmountChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TLCCAmountChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          LCC PercentChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TLCCPercentChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          LCC BaseChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TLCCBaseChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          LCC BasePercentChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TLCCBasePercentChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          EAA Total 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TEAACost_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          EAA AmountChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TEAAAmountChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          EAA PercentChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TEAAPercentChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          EAA BaseChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TEAABaseChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          EAA BasePercentChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TEAABasePercentChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Unit Total 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TUnitCost_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Unit AmountChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TUnitAmountChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Unit PercentChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TUnitPercentChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Unit BaseChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TUnitBaseChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
        <h4 class="ui-bar-b">
          Unit BasePercentChange 
        </h4>
			  <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			  <xsl:for-each select="@*">
				  <xsl:variable name="att_name" select="name()"/>
				  <xsl:variable name="att_value" select="."/>
				  <xsl:value-of select="DisplayComps:printValue('TUnitBasePercentChange_', $att_name, $att_value)"/>
			  </xsl:for-each>
        <div class="ui-grid-a">
			    <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
        </div>
      </div>
      <div data-role="collapsible" data-collapsed="true" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>SubCosts</strong>
        </h4>
        <xsl:if test="(@TSubP1Name1_0 != '' and @TSubP1Name1_0 != 'none') or (@TSubP1Name1_1 != '' and @TSubP1Name1_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 1 Name 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Name1_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Amount1_0 != '' and @TSubP1Amount1_0 != 'none') or (@TSubP1Amount1_1 != '' and @TSubP1Amount1_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 1 Amount 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Amount1_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Unit1_0 != '' and @TSubP1Unit1_0 != 'none') or (@TSubP1Unit1_1 != '' and @TSubP1Unit1_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 1 Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Unit1_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Price1_0 != '' and @TSubP1Price1_0 != 'none') or (@TSubP1Price1_1 != '' and @TSubP1Price1_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 1 Price 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Price1_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Total1_0 != '' and @TSubP1Total1_0 != 'none') or (@TSubP1Total1_1 != '' and @TSubP1Total1_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 1 Total 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Total1_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1TotalPerUnit1_0 != '' and @TSubP1TotalPerUnit1_0 != 'none') or (@TSubP1TotalPerUnit1_1 != '' and @TSubP1TotalPerUnit1_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 1 Total Per Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1TotalPerUnit1_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Name2_0 != '' and @TSubP1Name2_0 != 'none') or (@TSubP1Name2_1 != '' and @TSubP1Name2_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 2 Name 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Name2_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Amount2_0 != '' and @TSubP1Amount2_0 != 'none') or (@TSubP1Amount2_1 != '' and @TSubP1Amount2_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 2 Amount 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Amount2_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Unit2_0 != '' and @TSubP1Unit2_0 != 'none') or (@TSubP1Unit2_1 != '' and @TSubP1Unit2_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 2 Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Unit2_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Price2_0 != '' and @TSubP1Price2_0 != 'none') or (@TSubP1Price2_1 != '' and @TSubP1Price2_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 2 Price 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Price2_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Total2_0 != '' and @TSubP1Total2_0 != 'none') or (@TSubP1Total2_1 != '' and @TSubP1Total2_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 2 Total 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Total2_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1TotalPerUnit2_0 != '' and @TSubP1TotalPerUnit2_0 != 'none') or (@TSubP1TotalPerUnit2_1 != '' and @TSubP1TotalPerUnit2_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 2 Total Per Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1TotalPerUnit2_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Name3_0 != '' and @TSubP1Name3_0 != 'none') or (@TSubP1Name3_1 != '' and @TSubP1Name3_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 3 Name 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Name3_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Amount3_0 != '' and @TSubP1Amount3_0 != 'none') or (@TSubP1Amount3_1 != '' and @TSubP1Amount3_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 3 Amount 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Amount3_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Unit3_0 != '' and @TSubP1Unit3_0 != 'none') or (@TSubP1Unit3_1 != '' and @TSubP1Unit3_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 3 Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Unit3_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Price3_0 != '' and @TSubP1Price3_0 != 'none') or (@TSubP1Price3_1 != '' and @TSubP1Price3_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 3 Price 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Price3_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Total3_0 != '' and @TSubP1Total3_0 != 'none') or (@TSubP1Total3_1 != '' and @TSubP1Total3_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 3 Total 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Total3_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1TotalPerUnit3_0 != '' and @TSubP1TotalPerUnit3_0 != 'none') or (@TSubP1TotalPerUnit3_1 != '' and @TSubP1TotalPerUnit3_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 3 Total Per Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1TotalPerUnit3_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Name4_0 != '' and @TSubP1Name4_0 != 'none') or (@TSubP1Name4_1 != '' and @TSubP1Name4_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 4 Name 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Name4_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Amount4_0 != '' and @TSubP1Amount4_0 != 'none') or (@TSubP1Amount4_1 != '' and @TSubP1Amount4_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 4 Amount 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Amount4_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Unit4_0 != '' and @TSubP1Unit4_0 != 'none') or (@TSubP1Unit4_1 != '' and @TSubP1Unit4_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 4 Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Unit4_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Price4_0 != '' and @TSubP1Price4_0 != 'none') or (@TSubP1Price4_1 != '' and @TSubP1Price4_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 4 Price 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Price4_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Total4_0 != '' and @TSubP1Total4_0 != 'none') or (@TSubP1Total4_1 != '' and @TSubP1Total4_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 4 Total 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Total4_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1TotalPerUnit4_0 != '' and @TSubP1TotalPerUnit4_0 != 'none') or (@TSubP1TotalPerUnit4_1 != '' and @TSubP1TotalPerUnit4_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 4 Total Per Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1TotalPerUnit4_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Name5_0 != '' and @TSubP1Name5_0 != 'none') or (@TSubP1Name5_1 != '' and @TSubP1Name5_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 5 Name 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Name5_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Amount5_0 != '' and @TSubP1Amount5_0 != 'none') or (@TSubP1Amount5_1 != '' and @TSubP1Amount5_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 5 Amount 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Amount5_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Unit5_0 != '' and @TSubP1Unit5_0 != 'none') or (@TSubP1Unit5_1 != '' and @TSubP1Unit5_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 5 Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Unit5_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Price5_0 != '' and @TSubP1Price5_0 != 'none') or (@TSubP1Price5_1 != '' and @TSubP1Price5_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 5 Price 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Price5_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Total5_0 != '' and @TSubP1Total5_0 != 'none') or (@TSubP1Total5_1 != '' and @TSubP1Total5_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 5 Total 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Total5_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1TotalPerUnit5_0 != '' and @TSubP1TotalPerUnit5_0 != 'none') or (@TSubP1TotalPerUnit5_1 != '' and @TSubP1TotalPerUnit5_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 5 Total Per Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1TotalPerUnit5_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Name6_0 != '' and @TSubP1Name6_0 != 'none') or (@TSubP1Name6_1 != '' and @TSubP1Name6_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 6 Name 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Name6_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Amount6_0 != '' and @TSubP1Amount6_0 != 'none') or (@TSubP1Amount6_1 != '' and @TSubP1Amount6_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 6 Amount 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Amount6_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Unit6_0 != '' and @TSubP1Unit6_0 != 'none') or (@TSubP1Unit6_1 != '' and @TSubP1Unit6_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 6 Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Unit6_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Price6_0 != '' and @TSubP1Price6_0 != 'none') or (@TSubP1Price6_1 != '' and @TSubP1Price6_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 6 Price 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Price6_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Total6_0 != '' and @TSubP1Total6_0 != 'none') or (@TSubP1Total6_1 != '' and @TSubP1Total6_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 6 Total 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Total6_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1TotalPerUnit6_0 != '' and @TSubP1TotalPerUnit6_0 != 'none') or (@TSubP1TotalPerUnit6_1 != '' and @TSubP1TotalPerUnit6_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 6 Total Per Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1TotalPerUnit6_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Name7_0 != '' and @TSubP1Name7_0 != 'none') or (@TSubP1Name7_1 != '' and @TSubP1Name7_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 7 Name 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Name7_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Amount7_0 != '' and @TSubP1Amount7_0 != 'none') or (@TSubP1Amount7_1 != '' and @TSubP1Amount7_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 7 Amount 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Amount7_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Unit7_0 != '' and @TSubP1Unit7_0 != 'none') or (@TSubP1Unit7_1 != '' and @TSubP1Unit7_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 7 Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Unit7_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Price7_0 != '' and @TSubP1Price7_0 != 'none') or (@TSubP1Price7_1 != '' and @TSubP1Price7_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 7 Price 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Price7_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Total7_0 != '' and @TSubP1Total7_0 != 'none') or (@TSubP1Total7_1 != '' and @TSubP1Total7_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 7 Total 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Total7_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1TotalPerUnit7_0 != '' and @TSubP1TotalPerUnit7_0 != 'none') or (@TSubP1TotalPerUnit7_1 != '' and @TSubP1TotalPerUnit7_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 7 Total Per Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1TotalPerUnit8_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Name8_0 != '' and @TSubP1Name8_0 != 'none') or (@TSubP1Name8_1 != '' and @TSubP1Name8_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 8 Name 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Name8_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Amount8_0 != '' and @TSubP1Amount8_0 != 'none') or (@TSubP1Amount8_1 != '' and @TSubP1Amount8_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 8 Amount 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Amount8_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Unit8_0 != '' and @TSubP1Unit8_0 != 'none') or (@TSubP1Unit8_1 != '' and @TSubP1Unit8_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 8 Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Unit8_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Price8_0 != '' and @TSubP1Price8_0 != 'none') or (@TSubP1Price8_1 != '' and @TSubP1Price8_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 8 Price 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Price8_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Total8_0 != '' and @TSubP1Total8_0 != 'none') or (@TSubP1Total8_1 != '' and @TSubP1Total8_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 8 Total 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Total8_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1TotalPerUnit8_0 != '' and @TSubP1TotalPerUnit8_0 != 'none') or (@TSubP1TotalPerUnit8_1 != '' and @TSubP1TotalPerUnit8_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 8 Total Per Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1TotalPerUnit8_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Name9_0 != '' and @TSubP1Name9_0 != 'none') or (@TSubP1Name9_1 != '' and @TSubP1Name9_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 9 Name 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Name9_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Amount9_0 != '' and @TSubP1Amount9_0 != 'none') or (@TSubP1Amount9_1 != '' and @TSubP1Amount9_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 9 Amount 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Amount9_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Unit9_0 != '' and @TSubP1Unit9_0 != 'none') or (@TSubP1Unit9_1 != '' and @TSubP1Unit9_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 9 Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Unit9_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Price9_0 != '' and @TSubP1Price9_0 != 'none') or (@TSubP1Price9_1 != '' and @TSubP1Price9_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 9 Price 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Price9_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Total9_0 != '' and @TSubP1Total9_0 != 'none') or (@TSubP1Total9_1 != '' and @TSubP1Total9_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 9 Total 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Total9_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1TotalPerUnit9_0 != '' and @TSubP1TotalPerUnit9_0 != 'none') or (@TSubP1TotalPerUnit9_1 != '' and @TSubP1TotalPerUnit9_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 9 Total Per Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1TotalPerUnit9_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Name10_0 != '' and @TSubP1Name10_0 != 'none') or (@TSubP1Name10_1 != '' and @TSubP1Name10_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 10 Name 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Name10_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Amount10_0 != '' and @TSubP1Amount10_0 != 'none') or (@TSubP1Amount10_1 != '' and @TSubP1Amount10_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 10 Amount 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Amount10_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Unit10_0 != '' and @TSubP1Unit10_0 != 'none') or (@TSubP1Unit10_1 != '' and @TSubP1Unit10_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 10 Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Unit10_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Price10_0 != '' and @TSubP1Price10_0 != 'none') or (@TSubP1Price10_1 != '' and @TSubP1Price10_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 10 Price 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Price10_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1Total10_0 != '' and @TSubP1Total10_0 != 'none') or (@TSubP1Total10_1 != '' and @TSubP1Total10_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 10 Total 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1Total10_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
        <xsl:if test="(@TSubP1TotalPerUnit10_0 != '' and @TSubP1TotalPerUnit10_0 != 'none') or (@TSubP1TotalPerUnit10_1 != '' and @TSubP1TotalPerUnit10_1 != 'none')">
          <h4 class="ui-bar-b">
            SubCost 10 Total Per Unit 
          </h4>
			    <xsl:value-of select="DisplayComps:initValues($fullcolcount)"/>
			    <xsl:for-each select="@*">
				    <xsl:variable name="att_name" select="name()"/>
				    <xsl:variable name="att_value" select="."/>
				    <xsl:value-of select="DisplayComps:printValue('TSubP1TotalPerUnit10_', $att_name, $att_value)"/>
			    </xsl:for-each>
          <div class="ui-grid-a">
			      <xsl:value-of select="DisplayComps:DoPrintMobileValues($fullcolcount)"/>
          </div>
        </xsl:if>
      </div>
      </xsl:if>
	</xsl:template>
</xsl:stylesheet>
