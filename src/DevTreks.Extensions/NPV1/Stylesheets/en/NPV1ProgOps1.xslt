<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2013, December -->
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
      <strong>Operation Group</strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="operation">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="operation">
    <h4>
      <strong>Operation </strong>: <xsl:value-of select="@Name" />(Amount:&#xA0;<xsl:value-of select="@Amount" />;&#xA0;Date:&#xA0;<xsl:value-of select="@Date" />;&#xA0;Label:&#xA0;<xsl:value-of select="@Num" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
		<xsl:apply-templates select="operationinput">
			<xsl:sort select="@InputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="operationinput">
    <h4>
      <strong>Input </strong> : <xsl:value-of select="@Name" />
    </h4>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='npvprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
    <xsl:if test="($localName != 'operationinput')">
      <div data-role="collapsible" data-collapsed="false" data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Operation Details</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
            Date : <xsl:value-of select="@Date"/>
          </div>
          <div class="ui-block-b">
            Observations : <xsl:value-of select="@Observations"/>; Target Type : <xsl:value-of select="@TargetType"/>
          </div>
          <div class="ui-block-a">
            OC Planned Period : <xsl:value-of select="@TAMOC"/>
          </div>
          <div class="ui-block-b">
            OC Plan Full : <xsl:value-of select="@TAMOCPFTotal"/>
          </div>
          <div class="ui-block-a">
            OC Plan Cumul : <xsl:value-of select="@TAMOCPCTotal"/>
          </div>
          <div class="ui-block-b">
            OC Actual Period : <xsl:value-of select="@TAMOCAPTotal"/>
          </div>
          <div class="ui-block-a">
            OC Actual Cumul : <xsl:value-of select="@TAMOCACTotal"/>
          </div>
          <div class="ui-block-b">
            OC Actual Period Change : <xsl:value-of select="@TAMOCAPChange"/>
          </div>
          <div class="ui-block-a">
            OC Actual Cumul Change : <xsl:value-of select="@TAMOCACChange"/>
          </div>
          <div class="ui-block-b">
            OC Planned Period Percent : <xsl:value-of select="@TAMOCPPPercent"/>
          </div>
          <div class="ui-block-a">
            OC Planned Cumul Percent : <xsl:value-of select="@TAMOCPCPercent"/>
          </div>
          <div class="ui-block-b">
            OC Planned Full Percent : <xsl:value-of select="@TAMOCPFPercent"/>
          </div>
          <div class="ui-block-a">
            AOH Planned Period : <xsl:value-of select="@TAMAOH"/>
          </div>
          <div class="ui-block-b">
            AOH Plan Full : <xsl:value-of select="@TAMAOHPFTotal"/>
          </div>
          <div class="ui-block-a">
            AOH Plan Cumul : <xsl:value-of select="@TAMAOHPCTotal"/>
          </div>
          <div class="ui-block-b">
            AOH Actual Period : <xsl:value-of select="@TAMAOHAPTotal"/>
          </div>
          <div class="ui-block-a">
            AOH Actual Cumul : <xsl:value-of select="@TAMAOHACTotal"/>
          </div>
          <div class="ui-block-b">
            AOH Actual Period Change : <xsl:value-of select="@TAMAOHAPChange"/>
          </div>
          <div class="ui-block-a">
            AOH Actual Cumul Change : <xsl:value-of select="@TAMAOHACChange"/>
          </div>
          <div class="ui-block-b">
            AOH Planned Period Percent : <xsl:value-of select="@TAMAOHPPPercent"/>
          </div>
          <div class="ui-block-a">
            AOH Planned Cumul Percent : <xsl:value-of select="@TAMAOHPCPercent"/>
          </div>
          <div class="ui-block-b">
            AOH Planned Full Percent : <xsl:value-of select="@TAMAOHPFPercent"/>
          </div>
          <div class="ui-block-a">
            CAP Planned Period : <xsl:value-of select="@TAMCAP"/>
          </div>
          <div class="ui-block-b">
            CAP Plan Full : <xsl:value-of select="@TAMCAPPFTotal"/>
          </div>
          <div class="ui-block-a">
            CAP Plan Cumul : <xsl:value-of select="@TAMCAPPCTotal"/>
          </div>
          <div class="ui-block-b">
            CAP Actual Period : <xsl:value-of select="@TAMCAPAPTotal"/>
          </div>
          <div class="ui-block-a">
            CAP Actual Cumul : <xsl:value-of select="@TAMCAPACTotal"/>
          </div>
          <div class="ui-block-b">
            CAP Actual Period Change : <xsl:value-of select="@TAMCAPAPChange"/>
          </div>
          <div class="ui-block-a">
            CAP Actual Cumul Change : <xsl:value-of select="@TAMCAPACChange"/>
          </div>
          <div class="ui-block-b">
            CAP Planned Period Percent : <xsl:value-of select="@TAMCAPPPPercent"/>
          </div>
          <div class="ui-block-a">
            CAP Planned Cumul Percent : <xsl:value-of select="@TAMCAPPCPercent"/>
          </div>
          <div class="ui-block-b">
            CAP Planned Full Percent : <xsl:value-of select="@TAMCAPPFPercent"/>
          </div>
          <div class="ui-block-a">
            Total Planned Period : <xsl:value-of select="@TAMTOTAL"/>
          </div>
          <div class="ui-block-b">
            Total Plan Full : <xsl:value-of select="@TAMPFTotal"/>
          </div>
          <div class="ui-block-a">
            Total Plan Cumul : <xsl:value-of select="@TAMPCTotal"/>
          </div>
          <div class="ui-block-b">
            Total Actual Period : <xsl:value-of select="@TAMAPTotal"/>
          </div>
          <div class="ui-block-a">
            Total Actual Cumul : <xsl:value-of select="@TAMACTotal"/>
          </div>
          <div class="ui-block-b">
            Total Actual Period Change : <xsl:value-of select="@TAMAPChange"/>
          </div>
          <div class="ui-block-a">
            Total Actual Cumul Change : <xsl:value-of select="@TAMACChange"/>
          </div>
          <div class="ui-block-b">
            Total Planned Period Percent : <xsl:value-of select="@TAMPPPercent"/>
          </div>
          <div class="ui-block-a">
            Total Planned Cumul Percent : <xsl:value-of select="@TAMPCPercent"/>
          </div>
          <div class="ui-block-b">
            Total Planned Full Percent : <xsl:value-of select="@TAMPFPercent"/>
          </div>
          <div class="ui-block-a">
            Incent Planned Period : <xsl:value-of select="@TAMINCENT"/>
          </div>
          <div class="ui-block-b">
            Incent Plan Full : <xsl:value-of select="@TAMIncentPFTotal"/>
          </div>
          <div class="ui-block-a">
            Incent Plan Cumul : <xsl:value-of select="@TAMIncentPCTotal"/>
          </div>
          <div class="ui-block-b">
            Incent Actual Period : <xsl:value-of select="@TAMIncentAPTotal"/>
          </div>
          <div class="ui-block-a">
            Incent Actual Cumul : <xsl:value-of select="@TAMIncentACTotal"/>
          </div>
          <div class="ui-block-b">
            Incent Actual Period Change : <xsl:value-of select="@TAMIncentAPChange"/>
          </div>
          <div class="ui-block-a">
            Incent Actual Cumul Change : <xsl:value-of select="@TAMIncentACChange"/>
          </div>
          <div class="ui-block-b">
            Incent Planned Period Percent : <xsl:value-of select="@TAMIncentPPPercent"/>
          </div>
          <div class="ui-block-a">
            Incent Planned Cumul Percent : <xsl:value-of select="@TAMIncentPCPercent"/>
          </div>
          <div class="ui-block-b">
            Incent Planned Full Percent : <xsl:value-of select="@TAMIncentPFPercent"/>
          </div>
        </div>
      </div>
		</xsl:if>
		<xsl:if test="($localName = 'operationinput')">
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" >
        <h4 class="ui-bar-b">
          <strong>Input Details</strong>
        </h4>
        <div class="ui-grid-a">
          <div class="ui-block-a">
           Total OC : <xsl:value-of select="@TAMOC"/>
          </div>
          <div class="ui-block-b">
            Total AOH : <xsl:value-of select="@TAMAOH"/>
          </div>
          <div class="ui-block-a">
            Total CAP : <xsl:value-of select="@TAMCAP"/>
          </div>
          <div class="ui-block-b">
           Total Total : <xsl:value-of select="@TAMTOTAL"/>
          </div>
          <div class="ui-block-b">
            Total Incent : <xsl:value-of select="@TAMINCENT"/>
          </div>
        </div>
      </div>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
