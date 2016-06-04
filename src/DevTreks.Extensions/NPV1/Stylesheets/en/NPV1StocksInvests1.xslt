<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2013, December -->
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
      <strong>Investment Group</strong> :&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvtotal1']">
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
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvtotal1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="investmenttimeperiod">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmenttimeperiod">
		<h4>
      <strong>Time Period</strong> : <xsl:value-of select="@Name" />
    </h4>
    <xsl:apply-templates select="root/linkedview[@AnalyzerType='npvtotal1']">
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
		<h4>
      <strong>Outcome </strong> :&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvtotal1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="investmentoutput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentoutput">
    <h4>
      <strong>Output </strong> :&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvtotal1']">
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
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvtotal1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="investmentinput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="investmentinput">
		<h4>
      <strong>Input </strong> :&#xA0;<xsl:value-of select="@Name" />&#xA0;<xsl:value-of select="@Num" />&#xA0;<xsl:value-of select="@Date" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvtotal1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
		<xsl:if test="($localName != 'investmentinput' and $localName != 'investmentoutput')">
      <xsl:if test="($localName != 'investmentcomponent')">
        <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Benefit Details</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
           Output Name : <xsl:value-of select="@TRName"/>
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@TRUnit"/>
          </div>
          <div class="ui-block-a">
            Price : <xsl:value-of select="@TRPrice"/>
          </div>
          <div class="ui-block-b">
            Amount : <xsl:value-of select="@TRAmount"/>
          </div>
          <div class="ui-block-a">
            Compos Unit : <xsl:value-of select="@TRCompositionUnit"/>
          </div>
          <div class="ui-block-b">
            Compos Amount : <xsl:value-of select="@TRCompositionAmount"/>
          </div>
          <div class="ui-block-a">
           TBenefit : <xsl:value-of select="@TAMR"/>
          </div>
          <div class="ui-block-b">
            TR Incent : <xsl:value-of select="@TAMRINCENT"/>
          </div>
        </div>
      </div>
      </xsl:if>
      <xsl:if test="($localName != 'investmentoutcome')">
        <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Cost and Net Details</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
           TOC : <xsl:value-of select="@TAMOC"/>
          </div>
          <div class="ui-block-b">
           TAOH : <xsl:value-of select="@TAMAOH"/>
          </div>
          <div class="ui-block-a">
           TCAP : <xsl:value-of select="@TAMCAP"/>
          </div>
          <div class="ui-block-b">
           TCost : <xsl:value-of select="@TAMTOTAL"/>
          </div>
          <div class="ui-block-a">
            Net Returns : <xsl:value-of select="@TAMNET"/>
          </div>
          <div class="ui-block-b">
          </div>
           <div class="ui-block-a">
            TIncent : <xsl:value-of select="@TAMINCENT"/>
          </div>
          <div class="ui-block-b">
            Net Incent Returns: <xsl:value-of select="@TAMINCENT_NET"/>
          </div>
        </div>
      </div>
      </xsl:if>
		</xsl:if>
		<xsl:if test="($localName = 'investmentinput')">
			<div data-role="collapsible"  data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Input Details</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
           TAMOC : <xsl:value-of select="@TAMOC"/>
          </div>
          <div class="ui-block-b">
           TAMAOH : <xsl:value-of select="@TAMAOH"/>
          </div>
          <div class="ui-block-a">
           TAMCAP : <xsl:value-of select="@TAMCAP"/>
          </div>
          <div class="ui-block-b">
           TAMTOTAL : <xsl:value-of select="@TAMTOTAL"/>
          </div>
          <div class="ui-block-a">
            TAMIncent : <xsl:value-of select="@TAMINCENT"/>
          </div>
          <div class="ui-block-b">
          </div>
          <div class="ui-block-a">
            TOC P: <xsl:value-of select="@TOCPrice" />
          </div>
          <div class="ui-block-b">
            TOC Q : <xsl:value-of select="@TOCAmount" />
          </div>
          <div class="ui-block-a">
            TAOH P : <xsl:value-of select="@TAOHPrice" />
          </div>
          <div class="ui-block-a">
            TAOH Q : <xsl:value-of select="@TAOHAmount" />
          </div>
          <div class="ui-block-a">
            TCAP P : <xsl:value-of select="@TCAPPrice" />
          </div>
          <div class="ui-block-b">
            TCAP Q : <xsl:value-of select="@TCAPAmount" />
          </div>
        </div>
      </div>
		</xsl:if>
    <xsl:if test="($localName = 'investmentoutput')">
			<div data-role="collapsible"  data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Output Details</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
           Output Name : <xsl:value-of select="@TRName"/>
          </div>
          <div class="ui-block-b">
            Unit : <xsl:value-of select="@TRUnit"/>
          </div>
          <div class="ui-block-a">
            Price : <xsl:value-of select="@TRPrice"/>
          </div>
          <div class="ui-block-b">
            Amount : <xsl:value-of select="@TRAmount"/>
          </div>
          <div class="ui-block-a">
            Compos Unit : <xsl:value-of select="@TRCompositionUnit"/>
          </div>
          <div class="ui-block-b">
            Compos Amount : <xsl:value-of select="@TRCompositionAmount"/>
          </div>
          <div class="ui-block-a">
           Total Benefit : <xsl:value-of select="@TAMR"/>
          </div>
          <div class="ui-block-b">
            Total R Incent : <xsl:value-of select="@TAMRINCENT"/>
          </div>
        </div>
      </div>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>