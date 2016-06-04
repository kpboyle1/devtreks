<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2013, December -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:y0="urn:DevTreks-support-schemas:Outcome"
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
			<xsl:apply-templates select="outcomegroup" />
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
		<xsl:apply-templates select="outcomegroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outcomegroup">
		<h4>
      <strong>Outcome Group</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvchangeyr' 
      or @AnalyzerType='npvchangeid' or @AnalyzerType='npvchangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="outcome">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outcome">
    <h4>
      <strong>Outcome </strong> : <xsl:value-of select="@Name" />(Amount:&#xA0;<xsl:value-of select="@Amount" />;&#xA0;Date:&#xA0;<xsl:value-of select="@Date" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvchangeyr' 
      or @AnalyzerType='npvchangeid' or @AnalyzerType='npvchangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
		<xsl:apply-templates select="outcomeoutput">
			<xsl:sort select="@OutputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outcomeoutput">
    <h4>
      <strong>Output </strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvchangeyr' 
      or @AnalyzerType='npvchangeid' or @AnalyzerType='npvchangealt']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
    <xsl:if test="($localName != 'outcomeoutput')">
      <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Outcome Details</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Date : <xsl:value-of select="@Date"/>
          </div>
          <div class="ui-block-b">
            Observations : <xsl:value-of select="@Observations"/>; Alternative : <xsl:value-of select="@AlternativeType"/>
          </div>
          <div class="ui-block-a">
            Ben Total : <xsl:value-of select="@TAMR"/>
          </div>
          <div class="ui-block-b">
            Ben AmountChange : <xsl:value-of select="@TAMRAmountChange"/>
          </div>
          <div class="ui-block-a">
            Ben PercentChange : <xsl:value-of select="@TAMRPercentChange"/>
          </div>
          <div class="ui-block-b">
            Ben BaseChange : <xsl:value-of select="@TAMRBaseChange"/>
          </div>
          <div class="ui-block-a">
            Ben BasePercentChange : <xsl:value-of select="@TAMRBasePercentChange"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            OutputQ Total : <xsl:value-of select="@TRAmount"/>
          </div>
          <div class="ui-block-b">
            OutputQ AmountChange : <xsl:value-of select="@TRAmountChange"/>
          </div>
          <div class="ui-block-a">
            OutputQ PercentChange : <xsl:value-of select="@TRAmountPercentChange"/>
          </div>
          <div class="ui-block-b">
            OutputQ BaseChange : <xsl:value-of select="@TRAmountBaseChange"/>
          </div>
          <div class="ui-block-a">
            OutputQ BasePercentChange : <xsl:value-of select="@TRAmountBasePercentChange"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            RIncent Total : <xsl:value-of select="@TAMRINCENT"/>
          </div>
          <div class="ui-block-b">
            RIncent AmountChange : <xsl:value-of select="@TAMRIncentAmountChange"/>
          </div>
          <div class="ui-block-a">
            RIncent PercentChange : <xsl:value-of select="@TAMRIncentPercentChange"/>
          </div>
          <div class="ui-block-b">
            RIncent BaseChange : <xsl:value-of select="@TAMRIncentBaseChange"/>
          </div>
          <div class="ui-block-a">
            RIncent BasePercentChange : <xsl:value-of select="@TAMRIncentBasePercentChange"/>
          </div>
          <div class="ui-block-b">    
          </div>
          <div class="ui-block-a">
            RPrice Total : <xsl:value-of select="@TRPrice"/>
          </div>
          <div class="ui-block-b">
            RPrice AmountChange : <xsl:value-of select="@TRPriceAmountChange"/>
          </div>
          <div class="ui-block-a">
            RPrice PercentChange : <xsl:value-of select="@TRPricePercentChange"/>
          </div>
          <div class="ui-block-b">
            RPrice BaseChange : <xsl:value-of select="@TRPriceBaseChange"/>
          </div>
          <div class="ui-block-a">
            RPrice BasePercentChange : <xsl:value-of select="@TRPriceBasePercentChange"/>
          </div>
          <div class="ui-block-b">    
          </div>
        </div>
      </div>
		</xsl:if>
		<xsl:if test="($localName = 'outcomeoutput')">
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Output Details</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
           Total Benefit : <xsl:value-of select="@TAMR"/>
          </div>
          <div class="ui-block-b">
            Total OutputQ : <xsl:value-of select="@TRAmount"/>
          </div>
          <div class="ui-block-a">
            Total Incent : <xsl:value-of select="@TAMRINCENT"/>
          </div>
          <div class="ui-block-b">
           Total RPrice : <xsl:value-of select="@TRPrice"/>
          </div>
        </div>
      </div>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
