<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2016, August -->
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
	<!-- what is the start row or db connection? -->
	<xsl:param name="startRow" />
	<!-- what is the end row or storage connection? -->
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
		<xsl:apply-templates select="story" />
		<xsl:variable name="hasnodescount"><xsl:value-of select="count(story)"/></xsl:variable>
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
<xsl:template match="story">
    <xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPack:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
	<xsl:if test="($viewEditType = 'full')">
		<label for="story1title">Title:</label>
		<input id="story1title" type="text" data-mini="true">
			<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Title;string;250</xsl:attribute>
			<xsl:attribute name="value"><xsl:value-of select="@Title" /></xsl:attribute>
		</input>
		<label for="story1name">Name:</label>
		<input id="story1name" type="text" data-mini="true">
			<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Name;string;250</xsl:attribute>
			<xsl:attribute name="value"><xsl:value-of select="@Name" /></xsl:attribute>
		</input>
		<label for="story1label">Sort Label:</label>
		<input id="story1label" type="text" data-mini="true">
			<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Label;string;50</xsl:attribute>
			<xsl:attribute name="value"><xsl:value-of select="@Label" /></xsl:attribute>
		</input>
		<label for="story1datemod">Date Last Modified:</label>
		<input id="story1datemod" type="text">
			<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;DateLastModified;datetime;8</xsl:attribute>
			<xsl:attribute name="value"><xsl:value-of select="@DateLastModified" /></xsl:attribute>
		</input>
		<label for="story1description">Description:</label>
		<textarea id="story1description" class="Text200H100PCW">
			<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Description;string;2000</xsl:attribute>
			<xsl:value-of select="@Description" />
		</textarea>
    <xsl:variable name="adddefaultparams">'&amp;parentnode=<xsl:value-of select="$searchurl" />&amp;defaultnode=<xsl:value-of select="DisplayDevPack:GetURIPattern('00Default',1,$networkId,'storycontent','')" /><xsl:value-of select="$calcParams" />'</xsl:variable>
		<div class="ui-grid-a">
      <div class="ui-block-a">
        <xsl:value-of select="DisplayDevPack:MakeDevTreksButton('adddefault_content', 'SubmitButton1Enabled150', 'Add Content Node', $contenturipattern, $selectedFileURIPattern, 'postrequest', 'edit', 'adddefaults', 'none', $adddefaultparams)" />
      </div>
      <div class="ui-block-b">
        <label for="storycontentaddcontent">Number to add</label>
        <input id="storycontentaddcontent" type="text">
          <xsl:attribute name="name">adddefault_<xsl:value-of select="$searchurl" /> </xsl:attribute>
          <xsl:attribute name="value">0</xsl:attribute>
        </input>
      </div>
		</div>
	</xsl:if>
	<xsl:if test="($viewEditType != 'full')">
		<h2 class="ui-bar"><xsl:value-of select="@Title" />:&#xA0;<xsl:value-of select="@Name" /></h2>
		<xsl:if test="(@Description != '' and @Description != 'none')">
			<p>
				<xsl:value-of select="@Description" />
			</p>
		</xsl:if>
	</xsl:if>
	<xsl:apply-templates select="storycontent">
		<xsl:sort select="@Label"/>
	</xsl:apply-templates>
</xsl:template>
  
<xsl:template match="storycontent">
	<!--don't show default nodes used to make new node insertions-->
	<xsl:if test="(@Id != '1')">
		<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPack:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
		<xsl:if test="($viewEditType = 'full')">
			<h4 class="ui-bar-b">Content For Story</h4>
			<label for="storycontenttitle">Title:</label>
			<input id="storycontenttitle" type="text" data-mini="true">
				<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Title;string;250</xsl:attribute>
				<xsl:attribute name="value"><xsl:value-of select="@Title" /></xsl:attribute>
			</input>
			<label for="storycontentname">Name:</label>
			<input id="storycontentname" type="text" data-mini="true">
				<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Name;string;250</xsl:attribute>
				<xsl:attribute name="value"><xsl:value-of select="@Name" /></xsl:attribute>
			</input>
			<xsl:if test="(@Id != '1')">
        <fieldset data-role="controlgroup" data-type="horizontal" data-mini="true">
				<input type="radio" value="Delete">
          <xsl:attribute name="id"><xsl:value-of select="@Id" />storycontentdelete</xsl:attribute>
					<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Delete</xsl:attribute>
				</input>
        <label>
          <xsl:attribute name="for"><xsl:value-of select="@Id" />storycontentdelete</xsl:attribute>
          Del
        </label>
				<input type="radio" value="">
          <xsl:attribute name="id"><xsl:value-of select="@Id" />storycontentundelete</xsl:attribute>
					<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Delete</xsl:attribute>
				</input>
        <label>
          <xsl:attribute name="for"><xsl:value-of select="@Id" />storycontentundelete</xsl:attribute>
          UnDel
        </label>
        </fieldset>
			</xsl:if>
			<label for="storycontentlabel">Sort Label:</label>
			<input id="storycontentlabel" type="text" data-mini="true">
				<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Label;string;50</xsl:attribute>
				<xsl:attribute name="value"><xsl:value-of select="@Label" /></xsl:attribute>
			</input>
			<label for="storycontentdescription">Description</label>
			<textarea id="storycontentdescription" class="Text200H100PCW">
				<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Description;string;2000</xsl:attribute>
				<xsl:value-of select="@Description" />
			</textarea>
			<div data-role="controlgroup" data-type="horizontal" data-mini="true">
				<xsl:variable name="adddefaultparams">'&amp;parentnode=<xsl:value-of select="$searchurl" />&amp;defaultnode=<xsl:value-of select="DisplayDevPack:GetURIPattern('00Default',1,$networkId,'storymedia','')" /><xsl:value-of select="$calcParams" />'</xsl:variable>
				<xsl:value-of select="DisplayDevPack:MakeDevTreksButton('adddefault_media', 'SubmitButton1Enabled150', 'Add Media Node', $contenturipattern, $selectedFileURIPattern, 'postrequest', 'edit', 'adddefaults', 'none', $adddefaultparams)" />
				<xsl:variable name="adddefaultparams2">'&amp;parentnode=<xsl:value-of select="$searchurl" />&amp;defaultnode=<xsl:value-of select="DisplayDevPack:GetURIPattern('00Default',1,$networkId,'storylink','')" /><xsl:value-of select="$calcParams" />'</xsl:variable>
				<xsl:value-of select="DisplayDevPack:MakeDevTreksButton('adddefault_link', 'SubmitButton1Enabled150', 'Add Link Node', $contenturipattern, $selectedFileURIPattern, 'postrequest', 'edit', 'adddefaults', 'none', $adddefaultparams2)" />
				<xsl:variable name="adddefaultparams3">'&amp;parentnode=<xsl:value-of select="$searchurl" />&amp;defaultnode=<xsl:value-of select="DisplayDevPack:GetURIPattern('00Default',1,$networkId,'storylist','')" /><xsl:value-of select="$calcParams" />'</xsl:variable>
				<xsl:value-of select="DisplayDevPack:MakeDevTreksButton('adddefault_list', 'SubmitButton1Enabled150', 'Add List Node', $contenturipattern, $selectedFileURIPattern, 'postrequest', 'edit', 'adddefaults', 'none', $adddefaultparams3)" />
			</div>
      <label for="storycontentadd">Number to add</label>
      <input id="storycontentadd" type="text">
				<xsl:attribute name="name">adddefault_<xsl:value-of select="$searchurl" /></xsl:attribute>
				<xsl:attribute name="value">0</xsl:attribute>
			</input>
		</xsl:if>
		<xsl:if test="($viewEditType != 'full')">
			<xsl:if test="(@Title != '' and @Title !='none')">
				<h4 class="ui-bar-b">
					<xsl:value-of select="@Title" />
				</h4>
			</xsl:if>
			<xsl:if test="(@Name != '' and @Name !='none')">
				<strong>
					<xsl:value-of select="@Name" />
				</strong>
			</xsl:if>
			<xsl:if test="(@Description != '' and @Description !='none')">
				<p>
					<xsl:value-of select="@Description" />
				</p>
			</xsl:if>
		</xsl:if>
		<xsl:apply-templates select="storymedia">
			<xsl:sort select="@Label"/>
		</xsl:apply-templates>
		<ol>
			<xsl:apply-templates select="storylist">
				<xsl:sort select="@Label"/>
			</xsl:apply-templates>
		</ol>
		<xsl:apply-templates select="storylink">
			<xsl:sort select="@Label"/>
		</xsl:apply-templates>
	</xsl:if>
</xsl:template>
<xsl:template match="storymedia">
	<!--don't show default nodes used to make new node insertions-->
	<xsl:if test="(@Id != '1')">
		<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPack:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
		<xsl:if test="($viewEditType = 'full')">
			<h4 class="ui-bar-b">Media For Story</h4>
			<label for="storymedianame">Name:</label>
			<input id="storymedianame" type="text" data-mini="true">
				<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Name;string;250</xsl:attribute>
				<xsl:attribute name="value"><xsl:value-of select="@Name" /></xsl:attribute>
			</input>
			<xsl:if test="(@Id != '1')">
        <fieldset data-role="controlgroup" data-type="horizontal" data-mini="true">
          <input type="radio" value="Delete">
            <xsl:attribute name="id"><xsl:value-of select="@Id" />storymediadelete</xsl:attribute>
            <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Delete</xsl:attribute>
          </input>
          <label>
            <xsl:attribute name="for"><xsl:value-of select="@Id" />storymediadelete</xsl:attribute>
            Del
          </label>
          <input type="radio" value="">
            <xsl:attribute name="id"><xsl:value-of select="@Id" />storymediaundelete</xsl:attribute>
            <xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Delete</xsl:attribute>
          </input>
          <label>
            <xsl:attribute name="for"><xsl:value-of select="@Id" />storymediaundelete</xsl:attribute>
            UnDel
          </label>
        </fieldset>
			</xsl:if>
			<label for="storymedialabel">Sort Label:</label>
			<input id="storymedialabel" type="text" data-mini="true">
				<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Label;string;50</xsl:attribute>
				<xsl:attribute name="value"><xsl:value-of select="@Label" /></xsl:attribute>
			</input>
			<label for="storymediafilename">File Name:</label>
			<input id="storymediafilename" type="text">
				<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;FileName;string;250</xsl:attribute>
				<xsl:attribute name="value"><xsl:value-of select="@FileName" /></xsl:attribute>
			</input>
			<label for="storymedialongdescription">Long Description</label>
			<textarea id="storymedialongdescription" class="Text200H100PCW">
				<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;LongDescription;string;2000</xsl:attribute>
				<xsl:value-of select="@LongDescription" />
			</textarea>
		</xsl:if>
		<xsl:if test="($viewEditType != 'full') and @FileName != '' and @FileName != 'none'">
      <!--2.0.0 refactor uses linkedlistsarray-->
			  <xsl:variable name="imgparam"><xsl:value-of select="DisplayDevPack:GetResourceUrl2(@FileName, $resourceparenturi, $resourceparenturi2, 'false', $serverSubActionType, $linkedListsArray)"/></xsl:variable>
        <xsl:if test="($imgparam != '')">
          <xsl:variable name="filepath"><xsl:value-of select="DisplayDevPack:GetResourceParam('0', $imgparam)" /></xsl:variable>
          <xsl:variable name="mimetype"><xsl:value-of select="DisplayDevPack:GetMimeType($filepath)" /></xsl:variable>
          <xsl:choose>
				    <xsl:when test="(contains($mimetype, 'image'))">
              <img width="100%" height="100%">
					      <xsl:attribute name="src"><xsl:value-of select="$filepath" /></xsl:attribute>
					      <xsl:attribute name="alt"><xsl:value-of select="DisplayDevPack:GetResourceParam('1', $imgparam)" /></xsl:attribute>
					      <xsl:attribute name="id"><xsl:value-of select="@FileName" /><xsl:value-of select="@Id" /></xsl:attribute>
					      <xsl:attribute name="name"><xsl:value-of select="DisplayDevPack:GetResourceParam('2', $imgparam)" /></xsl:attribute>
				      </img>
            </xsl:when>
            <xsl:when test="(contains($mimetype, 'video'))">
              <xsl:variable name="thumbnailURI"><xsl:value-of select="DisplayDevPack:GetImagesUrl('devtreks-logo.jpg')"/></xsl:variable>
              <div itemprop="video" itemscope="" itemtype="http://schema.org/VideoObject">
                  <span>Video: <span itemprop="name"><strong><xsl:value-of select="DisplayDevPack:GetResourceParam('2', $imgparam)" /></strong></span></span>
                  <br />
                  <meta itemprop="thumbnailUrl">
                    <xsl:attribute name="content"><xsl:value-of select="$thumbnailURI" /></xsl:attribute>
                  </meta>
                  <meta itemprop="contentUrl">
                    <xsl:attribute name="content"><xsl:value-of select="$filepath" /></xsl:attribute>
                  </meta>
                  <video controls="" width="300" height="300">
                    <xsl:attribute name="poster"><xsl:value-of select="$thumbnailURI" /></xsl:attribute>
                    <xsl:attribute name="src"><xsl:value-of select="$filepath" /></xsl:attribute>
                    <xsl:attribute name="type"><xsl:value-of select="$mimetype" /></xsl:attribute> 
                  </video>
                  <div>
                      <span itemprop="description"><xsl:value-of select="DisplayDevPack:GetResourceParam('1', $imgparam)" /></span>
                  </div>
              </div>
            </xsl:when>
            <xsl:otherwise>
              <a class="ui-btn">
				        <xsl:attribute name="id"><xsl:value-of select="@FileName" /><xsl:value-of select="@Id" /></xsl:attribute>
					      <xsl:attribute name="name"><xsl:value-of select="DisplayDevPack:GetResourceParam('2', $imgparam)" /></xsl:attribute>
				        <xsl:attribute name="href"><xsl:value-of select="$filepath" /></xsl:attribute>
				        Download <xsl:value-of select="@FileName" />
			        </a>
            </xsl:otherwise>
          </xsl:choose>
			  </xsl:if>
			  <br />
			  <strong>
				  <xsl:value-of select="@Name" />
			  </strong>
			<xsl:if test="(@LongDescription != '' and @LongDescription !='none')">
				<p>
					<xsl:value-of select="@LongDescription" />
				</p>
			</xsl:if>
		</xsl:if>
	</xsl:if>
</xsl:template>
<xsl:template match="storylink">
	<!--don't show default nodes used to make new node insertions-->
	<xsl:if test="(@Id != '1')">
		<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPack:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
		<xsl:if test="($viewEditType = 'full')">
			<h4 class="ui-bar-b">Link For Story</h4>
			<label for="storylinkname">Name:</label>
			<input id="storylinkname" type="text">
				<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Name;string;250</xsl:attribute>
				<xsl:attribute name="value"><xsl:value-of select="@Name" /></xsl:attribute>
			</input>
			<xsl:if test="(@Id != '1')">
        <fieldset data-role="controlgroup" data-type="horizontal" data-mini="true">
				<input type="radio" value="Delete">
          <xsl:attribute name="id"><xsl:value-of select="@Id" />storylinkdelete</xsl:attribute>
					<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Delete</xsl:attribute>
				</input>
        <label>
          <xsl:attribute name="for"><xsl:value-of select="@Id" />storylinkdelete</xsl:attribute>
          Del
        </label>
				<input type="radio" value="">
          <xsl:attribute name="id"><xsl:value-of select="@Id" />storylinkundelete</xsl:attribute>
					<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Delete</xsl:attribute>
				</input>
        <label>
          <xsl:attribute name="for"><xsl:value-of select="@Id" />storylinkundelete</xsl:attribute>
          UnDel
        </label>
         </fieldset>
			</xsl:if>
			<label for="storylinklabel">Sort Label:</label>
			<input id="storylinklabel" type="text" data-mini="true">
				<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Label;string;50</xsl:attribute>
				<xsl:attribute name="value"><xsl:value-of select="@Label" /></xsl:attribute>
			</input>
			<label for="storylinkuri">URI:</label>
			<textarea id="storylinkuri" class="Text75H100PCW">
				<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Href;string;2000</xsl:attribute>
				<xsl:value-of select="@Href" />
			</textarea>
		</xsl:if>
		<xsl:if test="($viewEditType != 'full') and @Href != '' and @Href != 'none'">
			<a class="ui-btn">
				<xsl:attribute name="id"><xsl:value-of select="@Id" /></xsl:attribute>
				<xsl:attribute name="name"><xsl:value-of select="@Id" /></xsl:attribute>
				<xsl:attribute name="href"><xsl:value-of select="@Href" /></xsl:attribute>
				<xsl:value-of select="@Name" />
			</a>
		</xsl:if>
	</xsl:if>
</xsl:template>
<xsl:template match="storylist">
	<!--don't show default nodes used to make new node insertions-->
	<xsl:if test="(@Id != '1')">
		<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPack:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
		<li>
			<xsl:if test="($viewEditType = 'full')">
				<h4 class="ui-bar-b">List For Story</h4>
				<label for="storylistname">Name:</label>
				<input id="storylistname" type="text" data-mini="true">
					<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Name;string;250</xsl:attribute>
					<xsl:attribute name="value"><xsl:value-of select="@Name" /></xsl:attribute>
				</input>
				<xsl:if test="(@Id != '1')">
          <fieldset data-role="controlgroup" data-type="horizontal" data-mini="true">
					<input type="radio" value="Delete">
            <xsl:attribute name="id"><xsl:value-of select="@Id" />storylistdelete</xsl:attribute>
						<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Delete</xsl:attribute>
					</input>
          <label>
            <xsl:attribute name="for"><xsl:value-of select="@Id" />storylistdelete</xsl:attribute>
            Del
          </label>
					<input type="radio" value="">
            <xsl:attribute name="id"><xsl:value-of select="@Id" />storylistundelete</xsl:attribute>
						<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Delete</xsl:attribute>
					</input>
          <label>
            <xsl:attribute name="for"><xsl:value-of select="@Id" />storylistundelete</xsl:attribute>
            UnDel
          </label>
          </fieldset>
				</xsl:if>
				<label for="storylistlabel">Sort Label:</label>
				<input id="storylistlabel" type="text" data-mini="true">
					<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Label;string;50</xsl:attribute>
					<xsl:attribute name="value"><xsl:value-of select="@Label" /></xsl:attribute>
				</input>
				<label for="storylistdescription">Description</label>
				<textarea id="storylistdescription" class="Text200H100PCW">
					<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Description;string;2000</xsl:attribute>
					<xsl:value-of select="@Description" />
				</textarea>
			</xsl:if>
			<xsl:if test="($viewEditType != 'full')">
				<xsl:if test="(@Name != '' and @Name !='none')">
					<strong>
						<xsl:value-of select="@Name" />:
					</strong>
				</xsl:if>
				<xsl:if test="(@Description != '' and @Description !='none')">
					<xsl:value-of select="@Description" />
				</xsl:if>
			</xsl:if>
		</li>
	</xsl:if>
</xsl:template>
</xsl:stylesheet>
