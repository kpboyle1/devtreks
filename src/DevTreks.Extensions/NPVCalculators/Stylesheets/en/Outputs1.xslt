<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2012, December -->
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
	<!-- which node to start with?  (in the case of customdocs this will be a DEVDOCS_TYPES) -->
	<xsl:param name="nodeName" />
	<!-- which view to use? -->
	<xsl:param name="viewEditType" />
	<!-- is this a coordinator? -->
	<xsl:param name="memberRole" />
	<!-- what is the current uri? -->
	<xsl:param name="selectedFileURIPattern" />
	<!-- the addin being used -->
	<xsl:param name="calcDocURI" />
	<!-- the node being calculated (in the case of customdocs this will be a DEVDOCS_TYPES) -->
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
					<a id="aFeedback" name="Feedback" class="ui-btn">
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
			<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPacks:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
			<h4>
        <strong>Output Group</strong> : <xsl:value-of select="@Name" />
      </h4>
      <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
        <h4 class="ui-bar-b">
          <strong>Output Group Details</strong>
        </h4>
			  <div>
					  Document Status: <xsl:value-of select="@DocStatus" />
			  </div>
        <div >
				    Description : <xsl:value-of select="@Description" />
			  </div>
        <div class="ui-grid-a">
          <div class="ui-block-a">
				    Type: <xsl:value-of select="DisplayDevPacks:WriteSelectListForTypes($searchurl, @ServiceId, @TypeId, 'TypeId', $viewEditType)"/>
				  </div>
          <div class="ui-block-b">
					  Label : <xsl:value-of select="@Num" />
        </div>
		  </div>
    </div>
		<xsl:apply-templates select="output">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="output">
		<h4>
      <strong>Output</strong> : <xsl:value-of select="@Name" />
    </h4>
    <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
      <h4 class="ui-bar-b">
        <strong>Output Details</strong>
      </h4>
      <div class="ui-grid-a">
        <div class="ui-block-a">
          Last Changed : <xsl:value-of select="@LastChangedDate" />
        </div>
        <div class="ui-block-b">
          Label : <xsl:value-of select="@Num" />
        </div>
        <div class="ui-block-a">
          Date&#xA0;Received : <xsl:value-of select="@OutputDate" />
        </div>
        <div class="ui-block-b">
          Amount : <xsl:value-of select="@OutputAmount1" />
        </div>
        <div class="ui-block-a">
          Unit : <xsl:value-of select="@OutputUnit1" />
        </div>
        <div class="ui-block-b">
          Price : <xsl:value-of select="@OutputPrice1" />
        </div>
      </div>
		<div >
			Description : <xsl:value-of select="@Description" />
	  </div>
  </div>
<xsl:apply-templates select="outputseries">
	<xsl:sort select="@OutputDate"/>
</xsl:apply-templates>
</xsl:template>
<xsl:template match="outputseries">
    <h4>
      <strong>Output Series</strong> : <xsl:value-of select="@Name" />
    </h4>
    <div data-role="collapsible"  data-theme="b" data-content-theme="d" data-mini="true">
      <h4 class="ui-bar-b">
        <strong>Output Series Details</strong>
      </h4>
      <div class="ui-grid-a">
        <div class="ui-block-a">
          Last Changed : <xsl:value-of select="@LastChangedDate" />
        </div>
        <div class="ui-block-b">
          Label : <xsl:value-of select="@Num" />
        </div>
        <div class="ui-block-a">
          Date&#xA0;Received : <xsl:value-of select="@OutputDate" />
        </div>
        <div class="ui-block-b">
          Amount : <xsl:value-of select="@OutputAmount1" />
        </div>
        <div class="ui-block-a">
          Unit : <xsl:value-of select="@OutputUnit1" />
        </div>
        <div class="ui-block-b">
          Price : <xsl:value-of select="@OutputPrice1" />
        </div>
      </div>
		<div >
			Description : <xsl:value-of select="@Description" />
	  </div>
  </div>
	</xsl:template>
</xsl:stylesheet>

  