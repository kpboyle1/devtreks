<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2016, May -->
<xsl:stylesheet version="1.0" 
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:DisplayDevPack="urn:displaydevpacks"
	xmlns:devtreks="http://www.devtreks.org">
  <xsl:output method="html" indent="yes" omit-xml-declaration="yes" encoding="UTF-8" />
	<!-- pass in params -->
  <!--array holding references to constants, locals, lists ...-->
	<xsl:param name="linkedListsArray" />
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
	<!-- what is the club's email? -->
	<xsl:param name="clubEmail" />
	<!-- what is the current serviceid? -->
	<xsl:param name="contenturipattern" />
	<!-- path to resources -->
	<xsl:param name="fullFilePath" />
	<xsl:variable name="resourceparenturi">
		<xsl:value-of select="DisplayDevPack:GetResourceParentURI($selectedFileURIPattern, $calcParams, 'doctocalcuri')"/>
	</xsl:variable>
  <xsl:variable name="resourceparenturi2">
		<xsl:value-of select="DisplayDevPack:GetResourceParentURI($selectedFileURIPattern, $calcParams, 'linkedviewsuri')"/>
	</xsl:variable>
	<!-- init html -->
  <xsl:template match="@*|/|node()" />
  <xsl:template match="/">
    <xsl:apply-templates select="root" />
  </xsl:template>
<xsl:template match="root">
	<div>
    <xsl:if test="contains($nodeName, 'linkedview')">
		  <xsl:variable name="calcparams2">'<xsl:value-of select="$calcParams" />'</xsl:variable>
		  <xsl:value-of select="DisplayDevPack:WriteOpenInPanelLink($selectedFileURIPattern, $calcparams2, $serverActionType, $contenturipattern)"/>
    </xsl:if>
		<xsl:apply-templates select="meta" />
		<xsl:variable name="hasnodescount"><xsl:value-of select="count(meta)"/></xsl:variable>
		<xsl:if test="($hasnodescount = 0)">
			<p>
				<strong>
					This stylesheet can not convert this xml document. Please use a stylesheet that is appropriate for the xml document being used.
				</strong>
			</p>
		</xsl:if>
		<p>
			<a id="aFeedback" name="Feedback">
				<xsl:attribute name="href">mailto:<xsl:value-of select="$clubEmail" />?subject=<xsl:value-of select="$selectedFileURIPattern" /></xsl:attribute>
				Feedback about <xsl:value-of select="$selectedFileURIPattern" />
			</a>
		</p>
	</div>
</xsl:template>
<xsl:template match="meta">
  <xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPack:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
	<xsl:apply-templates select="title">
	</xsl:apply-templates>
  <xsl:apply-templates select="creator">
	</xsl:apply-templates>
  <xsl:apply-templates select="subject">
	</xsl:apply-templates>
  <xsl:apply-templates select="description">
	</xsl:apply-templates>
  <xsl:apply-templates select="publisher">
	</xsl:apply-templates>
  <xsl:apply-templates select="contributor">
	</xsl:apply-templates>
  <xsl:apply-templates select="date">
	</xsl:apply-templates>
  <xsl:apply-templates select="type">
	</xsl:apply-templates>
  <xsl:apply-templates select="format">
	</xsl:apply-templates>
  <xsl:apply-templates select="identifier">
	</xsl:apply-templates>
  <xsl:apply-templates select="source">
	</xsl:apply-templates>
  <xsl:apply-templates select="language">
	</xsl:apply-templates>
  <xsl:apply-templates select="relation">
	</xsl:apply-templates>
  <xsl:apply-templates select="coverage">
	</xsl:apply-templates>
  <xsl:apply-templates select="rights">
	</xsl:apply-templates>
</xsl:template>
  
<xsl:template match="title">
	<!--don't show default nodes used to make new node insertions-->
	<xsl:if test="(@Id != '1')">
		<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPack:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
    <xsl:if test="($viewEditType = 'full')">
      <h4 class="ui-bar-b">Title</h4>
			<label for="title">Title</label>
			<textarea id="title" class="Text200H100PCW">
				<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Description;string;2000</xsl:attribute>
				<xsl:value-of select="@Description" />
			</textarea>
		</xsl:if>
		<xsl:if test="($viewEditType != 'full')">
			<xsl:if test="(@Description != '' and @Description !='none')">
        <h4 class="ui-bar-b">Title</h4>
				<p>
					<xsl:value-of select="@Description" />
				</p>
			</xsl:if>
		</xsl:if>
	</xsl:if>
</xsl:template>
<xsl:template match="creator">
	<!--don't show default nodes used to make new node insertions-->
	<xsl:if test="(@Id != '1')">
		<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPack:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
    <xsl:if test="($viewEditType = 'full')">
      <h4 class="ui-bar-b">Creator</h4>
			<label for="creator">Creator</label>
			<textarea id="creator" class="Text200H100PCW">
				<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Description;string;2000</xsl:attribute>
				<xsl:value-of select="@Description" />
			</textarea>
		</xsl:if>
		<xsl:if test="($viewEditType != 'full')">
			<xsl:if test="(@Description != '' and @Description !='none')">
        <h4 class="ui-bar-b">Creator</h4>
				<p>
					<xsl:value-of select="@Description" />
				</p>
			</xsl:if>
		</xsl:if>
	</xsl:if>
</xsl:template>
<xsl:template match="subject">
	<!--don't show default nodes used to make new node insertions-->
	<xsl:if test="(@Id != '1')">
		<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPack:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
    <xsl:if test="($viewEditType = 'full')">
      <h4 class="ui-bar-b">Subject</h4>
			<label for="subject">Subject</label>
			<textarea id="subject" class="Text200H100PCW">
				<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Description;string;2000</xsl:attribute>
				<xsl:value-of select="@Description" />
			</textarea>
		</xsl:if>
		<xsl:if test="($viewEditType != 'full')">
			<xsl:if test="(@Description != '' and @Description !='none')">
        <h4 class="ui-bar-b">Subject</h4>
				<p>
					<xsl:value-of select="@Description" />
				</p>
			</xsl:if>
		</xsl:if>
	</xsl:if>
</xsl:template>
<xsl:template match="description">
	<!--don't show default nodes used to make new node insertions-->
	<xsl:if test="(@Id != '1')">
		<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPack:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
    <xsl:if test="($viewEditType = 'full')">
      <h4 class="ui-bar-b">Description</h4>
			<label for="description">Description</label>
			<textarea id="description" class="Text200H100PCW">
				<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Description;string;2000</xsl:attribute>
				<xsl:value-of select="@Description" />
			</textarea>
		</xsl:if>
		<xsl:if test="($viewEditType != 'full')">
			<xsl:if test="(@Description != '' and @Description !='none')">
        <h4 class="ui-bar-b">Description</h4>
				<p>
					<xsl:value-of select="@Description" />
				</p>
			</xsl:if>
		</xsl:if>
	</xsl:if>
</xsl:template>
<xsl:template match="publisher">
	<!--don't show default nodes used to make new node insertions-->
	<xsl:if test="(@Id != '1')">
		<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPack:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
    <xsl:if test="($viewEditType = 'full')">
      <h4 class="ui-bar-b">Publisher</h4>
			<label for="publisher">Publisher</label>
			<textarea id="publisher" class="Text200H100PCW">
				<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Description;string;2000</xsl:attribute>
				<xsl:value-of select="@Description" />
			</textarea>
		</xsl:if>
		<xsl:if test="($viewEditType != 'full')">
			<xsl:if test="(@Description != '' and @Description !='none')">
        <h4 class="ui-bar-b">Publisher</h4>
				<p>
					<xsl:value-of select="@Description" />
				</p>
			</xsl:if>
		</xsl:if>
	</xsl:if>
</xsl:template>
<xsl:template match="contributor">
	<!--don't show default nodes used to make new node insertions-->
	<xsl:if test="(@Id != '1')">
		<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPack:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
    <xsl:if test="($viewEditType = 'full')">
      <h4 class="ui-bar-b">Contributor</h4>
			<label for="contributor">Contributor</label>
			<textarea id="contributor" class="Text200H100PCW">
				<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Description;string;2000</xsl:attribute>
				<xsl:value-of select="@Description" />
			</textarea>
		</xsl:if>
		<xsl:if test="($viewEditType != 'full')">
			<xsl:if test="(@Description != '' and @Description !='none')">
        <h4 class="ui-bar-b">Contributor</h4>
				<p>
					<xsl:value-of select="@Description" />
				</p>
			</xsl:if>
		</xsl:if>
	</xsl:if>
</xsl:template>
<xsl:template match="date">
	<!--don't show default nodes used to make new node insertions-->
	<xsl:if test="(@Id != '1')">
		<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPack:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
    <xsl:if test="($viewEditType = 'full')">
      <h4 class="ui-bar-b">Date</h4>
			<label for="date">Date</label>
			<textarea id="date" class="Text200H100PCW">
				<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Description;string;2000</xsl:attribute>
				<xsl:value-of select="@Description" />
			</textarea>
		</xsl:if>
		<xsl:if test="($viewEditType != 'full')">
			<xsl:if test="(@Description != '' and @Description !='none')">
        <h4 class="ui-bar-b">Date</h4>
				<p>
					<xsl:value-of select="@Description" />
				</p>
			</xsl:if>
		</xsl:if>
	</xsl:if>
</xsl:template>
<xsl:template match="type">
	<!--don't show default nodes used to make new node insertions-->
	<xsl:if test="(@Id != '1')">
		<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPack:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
    <xsl:if test="($viewEditType = 'full')">
      <h4 class="ui-bar-b">Type</h4>
			<label for="type">Type</label>
			<textarea id="type" class="Text200H100PCW">
				<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Description;string;2000</xsl:attribute>
				<xsl:value-of select="@Description" />
			</textarea>
		</xsl:if>
		<xsl:if test="($viewEditType != 'full')">
			<xsl:if test="(@Description != '' and @Description !='none')">
        <h4 class="ui-bar-b">Type</h4>
				<p>
					<xsl:value-of select="@Description" />
				</p>
			</xsl:if>
		</xsl:if>
	</xsl:if>
</xsl:template>
<xsl:template match="format">
	<!--don't show default nodes used to make new node insertions-->
	<xsl:if test="(@Id != '1')">
		<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPack:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
    <xsl:if test="($viewEditType = 'full')">
      <h4 class="ui-bar-b">Format</h4>
			<label for="format">Format</label>
			<textarea id="format" class="Text200H100PCW">
				<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Description;string;2000</xsl:attribute>
				<xsl:value-of select="@Description" />
			</textarea>
		</xsl:if>
		<xsl:if test="($viewEditType != 'full')">
			<xsl:if test="(@Description != '' and @Description !='none')">
        <h4 class="ui-bar-b">Format</h4>
				<p>
					<xsl:value-of select="@Description" />
				</p>
			</xsl:if>
		</xsl:if>
	</xsl:if>
</xsl:template>
<xsl:template match="identifier">
	<!--don't show default nodes used to make new node insertions-->
	<xsl:if test="(@Id != '1')">
		<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPack:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
    <xsl:if test="($viewEditType = 'full')">
      <h4 class="ui-bar-b">Identifier</h4>
			<label for="identifier">Identifier</label>
			<textarea id="identifier" class="Text200H100PCW">
				<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Description;string;2000</xsl:attribute>
				<xsl:value-of select="@Description" />
			</textarea>
		</xsl:if>
		<xsl:if test="($viewEditType != 'full')">
			<xsl:if test="(@Description != '' and @Description !='none')">
        <h4 class="ui-bar-b">Identifier</h4>
				<p>
					<xsl:value-of select="@Description" />
				</p>
			</xsl:if>
		</xsl:if>
	</xsl:if>
</xsl:template>
<xsl:template match="source">
	<!--don't show default nodes used to make new node insertions-->
	<xsl:if test="(@Id != '1')">
		<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPack:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
    <xsl:if test="($viewEditType = 'full')">
      <h4 class="ui-bar-b">Source</h4>
			<label for="source">Source</label>
			<textarea id="source" class="Text200H100PCW">
				<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Description;string;2000</xsl:attribute>
				<xsl:value-of select="@Description" />
			</textarea>
		</xsl:if>
		<xsl:if test="($viewEditType != 'full')">
			<xsl:if test="(@Description != '' and @Description !='none')">
        <h4 class="ui-bar-b">Source</h4>
				<p>
					<xsl:value-of select="@Description" />
				</p>
			</xsl:if>
		</xsl:if>
	</xsl:if>
</xsl:template>
<xsl:template match="language">
	<!--don't show default nodes used to make new node insertions-->
	<xsl:if test="(@Id != '1')">
		<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPack:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
    <xsl:if test="($viewEditType = 'full')">
      <h4 class="ui-bar-b">Language</h4>
			<label for="language">Language</label>
			<textarea id="language" class="Text200H100PCW">
				<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Description;string;2000</xsl:attribute>
				<xsl:value-of select="@Description" />
			</textarea>
		</xsl:if>
		<xsl:if test="($viewEditType != 'full')">
			<xsl:if test="(@Description != '' and @Description !='none')">
        <h4 class="ui-bar-b">Language</h4>
				<p>
					<xsl:value-of select="@Description" />
				</p>
			</xsl:if>
		</xsl:if>
	</xsl:if>
</xsl:template>
<xsl:template match="relation">
	<!--don't show default nodes used to make new node insertions-->
	<xsl:if test="(@Id != '1')">
		<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPack:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
    <xsl:if test="($viewEditType = 'full')">
      <h4 class="ui-bar-b">Relation</h4>
			<label for="relation">Relation</label>
			<textarea id="relation" class="Text200H100PCW">
				<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Description;string;2000</xsl:attribute>
				<xsl:value-of select="@Description" />
			</textarea>
		</xsl:if>
		<xsl:if test="($viewEditType != 'full')">
			<xsl:if test="(@Description != '' and @Description !='none')">
        <h4 class="ui-bar-b">Relation</h4>
				<p>
					<xsl:value-of select="@Description" />
				</p>
			</xsl:if>
		</xsl:if>
	</xsl:if>
</xsl:template>
<xsl:template match="coverage">
	<!--don't show default nodes used to make new node insertions-->
	<xsl:if test="(@Id != '1')">
		<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPack:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
    <xsl:if test="($viewEditType = 'full')">
      <h4 class="ui-bar-b">Coverage</h4>
			<label for="coverage">Coverage</label>
			<textarea id="coverage" class="Text200H100PCW">
				<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Description;string;2000</xsl:attribute>
				<xsl:value-of select="@Description" />
			</textarea>
		</xsl:if>
		<xsl:if test="($viewEditType != 'full')">
			<xsl:if test="(@Description != '' and @Description !='none')">
        <h4 class="ui-bar-b">Coverage</h4>
				<p>
					<xsl:value-of select="@Description" />
				</p>
			</xsl:if>
		</xsl:if>
	</xsl:if>
</xsl:template>
<xsl:template match="rights">
	<!--don't show default nodes used to make new node insertions-->
	<xsl:if test="(@Id != '1')">
		<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPack:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
    <xsl:if test="($viewEditType = 'full')">
      <h4 class="ui-bar-b">Rights</h4>
			<label for="rights">Rights</label>
			<textarea id="rights" class="Text200H100PCW">
				<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Description;string;2000</xsl:attribute>
				<xsl:value-of select="@Description" />
			</textarea>
		</xsl:if>
		<xsl:if test="($viewEditType != 'full')">
			<xsl:if test="(@Description != '' and @Description !='none')">
        <h4 class="ui-bar-b">Rights</h4>
				<p>
					<xsl:value-of select="@Description" />
				</p>
			</xsl:if>
		</xsl:if>
	</xsl:if>
</xsl:template>
</xsl:stylesheet>