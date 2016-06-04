<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2013, December -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Budget"
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
			<xsl:apply-templates select="budgetgroup" />
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
		<xsl:apply-templates select="budgetgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetgroup">
    <h4>
      <strong>Budget Group</strong>&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvstat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budget">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budget">
		<h4>
      <strong>Budget</strong> :&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvstat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budgettimeperiod">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgettimeperiod">
		<h4>
      <strong>Time Period</strong> :&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" />
    </h4>
    <xsl:apply-templates select="root/linkedview[@AnalyzerType='npvstat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
		<xsl:apply-templates select="budgetoutcomes" />
		<xsl:apply-templates select="budgetoperations" />
	</xsl:template>
	<xsl:template match="budgetoutcomes">
		<xsl:apply-templates select="budgetoutcome">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoutcome">
		<h4>
      <strong>Outcome </strong> :&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvstat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoperations">
		<xsl:apply-templates select="budgetoperation">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoperation">
		<h4>
      <strong>Operation</strong> :&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvstat1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
      <xsl:if test="($localName != 'budgetoperation')">
        <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
          <h4 class="ui-bar-b">
            <strong>Benefit Details</strong>
          </h4>
          <div class="ui-grid-a">
          <div class="ui-block-a">
            Benefit Observations : <xsl:value-of select="@TBenefitN"/>
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
            Benefit Total : <xsl:value-of select="@TAMR"/>
          </div>
          <div class="ui-block-b">
            Benefit Mean : <xsl:value-of select="@TAMR_MEAN"/>
          </div>
          <div class="ui-block-a">
            Benefit Median : <xsl:value-of select="@TAMR_MED"/>
          </div>
          <div class="ui-block-b">
            Benefit Var : <xsl:value-of select="@TAMR_VAR2"/>
          </div>
          <div class="ui-block-a">
            Benefit Std Dev : <xsl:value-of select="@TAMR_SD"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            Incent Total : <xsl:value-of select="@TAMRINCENT"/>
          </div>
          <div class="ui-block-b">
            Incent Mean : <xsl:value-of select="@TAMRINCENT_MEAN"/>
          </div>
          <div class="ui-block-a">
            Incent Median : <xsl:value-of select="@TAMRINCENT_MED"/>
          </div>
          <div class="ui-block-b">
            Incent Var : <xsl:value-of select="@TAMRINCENT_VAR2"/>
          </div>
          <div class="ui-block-a">
            Incent Std Dev : <xsl:value-of select="@TAMRINCENT_SD"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            Amount Total : <xsl:value-of select="@TRAmount"/>
          </div>
          <div class="ui-block-b">
            Amount Mean : <xsl:value-of select="@TRAmount_MEAN"/>
          </div>
          <div class="ui-block-a">
            Amount Median : <xsl:value-of select="@TRAmount_MED"/>
          </div>
          <div class="ui-block-b">
            Amount Var : <xsl:value-of select="@TRAmount_VAR2"/>
          </div>
          <div class="ui-block-a">
            Amount Std Dev : <xsl:value-of select="@TRAmount_SD"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            Comp Amount Total : <xsl:value-of select="@TRCompositionAmount"/>
          </div>
          <div class="ui-block-b">
            Comp Amount Mean : <xsl:value-of select="@TRCompositionAmount_MEAN"/>
          </div>
          <div class="ui-block-a">
            Comp Amount Median : <xsl:value-of select="@TRCompositionAmount_MED"/>
          </div>
          <div class="ui-block-b">
            Comp Amount Var : <xsl:value-of select="@TRCompositionAmount_VAR2"/>
          </div>
          <div class="ui-block-a">
            Comp Amount Std Dev : <xsl:value-of select="@TRCompositionAmount_SD"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            Price Total : <xsl:value-of select="@TRPrice"/>
          </div>
          <div class="ui-block-b">
            Price Mean : <xsl:value-of select="@TRPrice_MEAN"/>
          </div>
          <div class="ui-block-a">
            Price Median : <xsl:value-of select="@TRPrice_MED"/>
          </div>
          <div class="ui-block-b">
            Price Var : <xsl:value-of select="@TRPrice_VAR2"/>
          </div>
          <div class="ui-block-a">
            Price Std Dev : <xsl:value-of select="@TRPrice_SD"/>
          </div>
          <div class="ui-block-b">    
          </div>
        </div>
        </div>
      </xsl:if>
      <xsl:if test="($localName != 'budgetoutcome')">
        <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Cost Details</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Cost Observations : <xsl:value-of select="@TCostN"/>
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
            OC Total : <xsl:value-of select="@TAMOC"/>
          </div>
          <div class="ui-block-b">
            OC Mean : <xsl:value-of select="@TAMOC_MEAN"/>
          </div>
          <div class="ui-block-a">
            OC Median : <xsl:value-of select="@TAMOC_MED"/>
          </div>
          <div class="ui-block-b">
            OC Var : <xsl:value-of select="@TAMOC_VAR2"/>
          </div>
          <div class="ui-block-a">
            OC Std Dev : <xsl:value-of select="@TAMOC_SD"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            AOH Total : <xsl:value-of select="@TAMAOH"/>
          </div>
          <div class="ui-block-b">
            AOH Mean : <xsl:value-of select="@TAMAOH_MEAN"/>
          </div>
          <div class="ui-block-a">
            AOH Median : <xsl:value-of select="@TAMAOH_MED"/>
          </div>
          <div class="ui-block-b">
            AOH Var : <xsl:value-of select="@TAMAOH_VAR2"/>
          </div>
          <div class="ui-block-a">
            AOH Std Dev : <xsl:value-of select="@TAMAOH_SD"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            CAP Total : <xsl:value-of select="@TAMCAP"/>
          </div>
          <div class="ui-block-b">
            CAP Mean : <xsl:value-of select="@TAMCAP_MEAN"/>
          </div>
          <div class="ui-block-a">
            CAP Median : <xsl:value-of select="@TAMCAP_MED"/>
          </div>
          <div class="ui-block-b">
            CAP Var : <xsl:value-of select="@TAMCAP_VAR2"/>
          </div>
          <div class="ui-block-a">
            CAP Std Dev : <xsl:value-of select="@TAMCAP_SD"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            Total Cost : <xsl:value-of select="@TAMTOTAL"/>
          </div>
          <div class="ui-block-b">
            Total Mean : <xsl:value-of select="@TAMTOTAL_MEAN"/>
          </div>
          <div class="ui-block-a">
            Total Median : <xsl:value-of select="@TAMTOTAL_MED"/>
          </div>
          <div class="ui-block-b">
            Total Var : <xsl:value-of select="@TAMTOTAL_VAR2"/>
          </div>
          <div class="ui-block-a">
            Total Std Dev : <xsl:value-of select="@TAMTOTAL_SD"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            Incent : <xsl:value-of select="@TAMINCENT"/>
          </div>
          <div class="ui-block-b">
            Incent Mean : <xsl:value-of select="@TAMINCENT_MEAN"/>
          </div>
          <div class="ui-block-a">
            Incent Median : <xsl:value-of select="@TAMINCENT_MED"/>
          </div>
          <div class="ui-block-b">
            Incent Var : <xsl:value-of select="@TAMINCENT_VAR2"/>
          </div>
          <div class="ui-block-a">
            Incent Std Dev : <xsl:value-of select="@TAMINCENT_SD"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            Net : <xsl:value-of select="@TAMNET"/>
          </div>
          <div class="ui-block-b">
            Net Mean : <xsl:value-of select="@TAMNET_MEAN"/>
          </div>
          <div class="ui-block-a">
            Net Median : <xsl:value-of select="@TAMNET_MED"/>
          </div>
          <div class="ui-block-b">
            Net Var : <xsl:value-of select="@TAMNET_VAR2"/>
          </div>
          <div class="ui-block-a">
            Net Std Dev : <xsl:value-of select="@TAMNET_SD"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            Incent Net : <xsl:value-of select="@TAMINCENT_NET"/>
          </div>
          <div class="ui-block-b">
            Incent Net Mean : <xsl:value-of select="@TAMINCENT_NET_MEAN"/>
          </div>
          <div class="ui-block-a">
            Incent Net Median : <xsl:value-of select="@TAMINCENT_NET_MED"/>
          </div>
          <div class="ui-block-b">
            Incent Net Var : <xsl:value-of select="@TAMINCENT_NET_VAR2"/>
          </div>
          <div class="ui-block-a">
            Incent Net Std Dev : <xsl:value-of select="@TAMINCENT_NET_SD"/>
          </div>
          <div class="ui-block-b">    
          </div>
        </div>
      </div>
      </xsl:if>
      <div >
			  <strong>Description : </strong><xsl:value-of select="@Description" />
	    </div>
	</xsl:template>
</xsl:stylesheet>
