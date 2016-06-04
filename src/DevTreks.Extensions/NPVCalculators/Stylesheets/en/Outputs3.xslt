<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2013, January -->
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
		<div id="mainwrapper">
			<table class="data" cellpadding="6" cellspacing="1" border="0">
				<tbody>
					<xsl:apply-templates select="servicebase" />
					<xsl:apply-templates select="outputgroup" />
					<tr id="footer">
						<td scope="row" colspan="5">
							<a id="aFeedback" name="Feedback">
								<xsl:attribute name="href">mailto:<xsl:value-of select="$clubEmail" />?subject=<xsl:value-of select="$selectedFileURIPattern" /></xsl:attribute>
								Feedback About <xsl:value-of select="$selectedFileURIPattern" />
							</a>
						</td>
					</tr>
				</tbody>
			</table>
		</div>
	</xsl:template>
	<xsl:template match="servicebase">
		<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPacks:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
		<tr>
			<th colspan="5">
				Service: <xsl:value-of select="@Name" />
			</th>
		</tr>
		<xsl:apply-templates select="outputgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outputgroup">
			<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPacks:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
			<tr>
			<th scope="col" colspan="8">
				Output Group : <xsl:value-of select="@Name" />
			</th>
		</tr>
		<tr>
			<td scope="row" colspan="8">
					Document Status : <xsl:value-of select="DisplayDevPacks:WriteSelectListForStatus($searchurl, @DocStatus, $viewEditType)"/>
			</td>
		</tr>
		<tr>
			<td colspan="8">
					Description : <xsl:value-of select="@Description" />
			</td>
		</tr>
		<tr>
      <td scope="row" colspan="4">
        Label : <xsl:value-of select="@Num" />
      </td>
			<td scope="row" colspan="4">
				Output Type : <xsl:value-of select="DisplayDevPacks:WriteSelectListForTypes($searchurl, @ServiceId, @TypeId, 'TypeId', $viewEditType)"/>
			</td>
		</tr>
		<xsl:apply-templates select="output">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="output">
		<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPacks:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
		<xsl:if test="(contains($selectedFileURIPattern, '/temp')) or 
				($nodeName != 'outputgroup') or (($nodeName = 'outputgroup') and ((position() &gt;= $startRow) and (position() &lt;= $endRow)))">
			<xsl:variable name="seriesCount" select="count(outputseries)"/>
			<xsl:if test="(position() &gt;= 2) and ($seriesCount != 0)"></xsl:if>
			<tr>
				<th scope="col" colspan="5">Output</th>
			</tr>
			<tr>
				<th scope="col"><label for="lblOutputLabel" >Label</label></th>
				<th scope="col"><label for="lblOutputDate" >Date&#xA0;Received</label></th>
				<th scope="col"><label for="lblOutputAmount" >Amount</label></th>
				<th scope="col"><label for="lblOutputUnit" >Unit</label></th>
				<th scope="col"><label for="lblOutputPrice" >Price</label></th>
			</tr>
			<tr>
				<td colspan="5">
					<strong>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<strong><xsl:value-of select="@Name" /></strong> (<xsl:value-of select="@LastChangedDate" />)
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" class="Input400Bold" id="lblName">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Name;string;75</xsl:attribute>
							<xsl:attribute name="value">
								<xsl:value-of select="@Name" />
							</xsl:attribute>
						</input>
					</xsl:if></strong>
					<xsl:if test="($viewEditType = 'full')">
						<input class="Input25" type="radio" value="Delete">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Delete</xsl:attribute>Del
						</input>
						<input class="Input25" type="radio" value="">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Delete</xsl:attribute>UnDel
						</input>
					</xsl:if>
				</td>
			</tr>
			<tr>
				<td>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<xsl:value-of select="@Num" />
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" class="Input75Center" id="lblOutputLabel">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Num;string;25</xsl:attribute>
							<xsl:attribute name="value">
								<xsl:value-of select="@Num" />
							</xsl:attribute>
						</input>
					</xsl:if>
				</td>
				<td>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<xsl:value-of select="@OutputDate" />
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" class="Input75Center" id="lblOutputDate">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OutputDate;datetime;8</xsl:attribute>
							<xsl:attribute name="value">
								<xsl:value-of select="@OutputDate" />
							</xsl:attribute>
						</input>
					</xsl:if>
				</td>
				<td>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('OutputAmount1', @OutputAmount1, 'double', '8')"/>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" class="Input75Center" id="lblOutputAmount">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OutputAmount1;double;4</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('OutputAmount1', @OutputAmount1, 'double', '8')"/></xsl:attribute>
						</input>
					</xsl:if>
				</td>
				<td>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<xsl:value-of select="@OutputUnit1" />
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<select class="SelectUnits" id="lblOutputUnit">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OutputUnit1;string;75</xsl:attribute>
							<xsl:attribute name="data-unitid"><xsl:value-of select="@UnitGroupId" /></xsl:attribute>
							<option>
								<xsl:attribute name="value"><xsl:value-of select="@OutputUnit1" /></xsl:attribute>
								<xsl:attribute name="selected" />
								<xsl:value-of select="@OutputUnit1" />
							</option>
						</select>
					</xsl:if>
				</td>
				<td>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<xsl:value-of select="@OutputPrice1" />
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" class="Input75Center" id="lblOutputPrice">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OutputPrice1;decimal;8</xsl:attribute>
							<xsl:attribute name="value">
								<xsl:value-of select="@OutputPrice1" />
							</xsl:attribute>
						</input>
					</xsl:if>
				</td>
			</tr>
			<xsl:if test="($nodeName = 'output')">
				<tr>
					<td colspan="5">
						<label for="lblDescription" ><strong>Description</strong></label>
						<br />
						<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
							<xsl:value-of select="@Description" />
						</xsl:if>
						<xsl:if test="($viewEditType = 'full')">
							<textarea class="Text75H100PCW" id="lblDescription">
								<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Description;string;255</xsl:attribute>
								<xsl:value-of select="@Description" />
							</textarea>
						</xsl:if>
					</td>
				</tr>
			</xsl:if>
			<xsl:if test="(contains($selectedFileURIPattern, '/temp')) or (($nodeName = 'output') and ($viewEditType = 'print' or $viewEditType = 'full')
					and ($serverActionType != 'linkedviews'))">
				<tr>
					<td scope="row" colspan="5">
						<xsl:value-of select="DisplayDevPacks:WriteViewLinks($searchurl, $contenturipattern, $calcParams, local-name(), @OldId)"/>
					</td>
				</tr>
				<xsl:if test="($viewEditType = 'full')">
					<xsl:variable name="adddefaultparams">'&amp;parentnode=<xsl:value-of select="$searchurl" />&amp;defaultnode=<xsl:value-of select="$searchurl" /><xsl:value-of select="$calcParams" />'</xsl:variable>
					<tr>
						<td colspan="5">
							<xsl:value-of select="DisplayDevPacks:MakeDevTreksButton('adddefault_outputseries', 'SubmitButton1Enabled150', 'Add Output To Output Series', $contenturipattern, $selectedFileURIPattern, 'postrequest', 'edit', 'adddefaults', 'none', $adddefaultparams)" />
							&#xA0;<input type="text" class="Input35">
								<xsl:attribute name="name">adddefault_<xsl:value-of select="$searchurl" /></xsl:attribute>
								<xsl:attribute name="value">0</xsl:attribute>
							</input>&#xA0;Number to add
						</td>
					</tr>
				</xsl:if>
			</xsl:if>
			<xsl:if test="($seriesCount != 0)">
				<tr>
					<th scope="col" colspan="5">Output Series</th>
				</tr>
				<xsl:if test="($viewEditType = 'full') and (contains($fullFilePath, '_temp'))">
					<tr>
						<td colspan="5">
							<xsl:value-of select="DisplayDevPacks:MakeGetSelectionsLink('selectexisting1', '#',
								'GetSelectsLink', 'Select Output Series', 'spanSelectionFiles', 
								$contenturipattern, $searchurl, 'outputseries', '', $calcParams)" />
						</td>
					</tr>
				</xsl:if>
				<xsl:apply-templates select="outputseries">
					<xsl:sort select="@OutputDate"/>
				</xsl:apply-templates>
			</xsl:if>
		</xsl:if>
	</xsl:template>
	<xsl:template match="outputseries">
		<xsl:if test="(contains($selectedFileURIPattern, '/temp')) or 
				($nodeName != 'output') or (($nodeName = 'output') and ((position() &gt;= $startRow) and (position() &lt;= $endRow)))">
			<xsl:variable name="searchurl"><xsl:value-of select="DisplayDevPacks:GetURIPattern(@Name,@Id,$networkId,local-name(),'')" /></xsl:variable>
			<tr>
				<td scope="row" colspan="5">
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<strong><xsl:value-of select="@Name" /></strong>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" class="Input400Bold" id="lblOutputSeriesName">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Name;string;255</xsl:attribute>
							<xsl:attribute name="value">
								<xsl:value-of select="@Name" />
							</xsl:attribute>
						</input>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input class="Input25" type="radio" value="Delete">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Delete</xsl:attribute>Del
						</input>
						<input class="Input25" type="radio" value="">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Delete</xsl:attribute>UnDel
						</input>
					</xsl:if>
				</td>
			</tr>
			<tr>
				<td>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<xsl:value-of select="@Num" />
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" class="Input75Center" id="lblOutputSeriesLabel">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Num;string;25</xsl:attribute>
							<xsl:attribute name="value">
								<xsl:value-of select="@Num" />
							</xsl:attribute>
						</input>
					</xsl:if>
				</td>
				<td>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<xsl:value-of select="@OutputDate" />
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" class="Input75Center" id="lblOutputSeriesDate">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OutputDate;datetime;8</xsl:attribute>
							<xsl:attribute name="value">
								<xsl:value-of select="@OutputDate" />
							</xsl:attribute>
						</input>
					</xsl:if>
				</td>
				<td>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('OutputAmount1', @OutputAmount1, 'double', '8')"/>
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" class="Input75Center" id="lblOutputSeriesAmount">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OutputAmount1;double;8</xsl:attribute>
							<xsl:attribute name="value"><xsl:value-of select="DisplayDevPacks:WriteFormattedNumber('OutputAmount1', @OutputAmount1, 'double', '8')"/></xsl:attribute>
						</input>
					</xsl:if>
				</td>
				<td>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<xsl:value-of select="@OutputUnit1" />
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<select class="SelectUnits" id="lblOutputSeriesUnit">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OutputUnit1;string;75</xsl:attribute>
							<xsl:attribute name="data-unitid"><xsl:value-of select="@UnitGroupId" /></xsl:attribute>
							<option>
								<xsl:attribute name="value"><xsl:value-of select="@OutputUnit1" /></xsl:attribute>
								<xsl:attribute name="selected" />
								<xsl:value-of select="@OutputUnit1" />
							</option>
						</select>
					</xsl:if>
				</td>
				<td>
					<xsl:if test="($viewEditType = 'print') or ($viewEditType = 'part')">
						<xsl:value-of select="@OutputPrice1" />
					</xsl:if>
					<xsl:if test="($viewEditType = 'full')">
						<input type="text" class="Input75Center" id="lblOutputSeriesPrice">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;OutputPrice1;decimal;8</xsl:attribute>
							<xsl:attribute name="value">
								<xsl:value-of select="@OutputPrice1" />
							</xsl:attribute>
						</input>
					</xsl:if>
				</td>
			</tr>
			<tr>
				<td colspan="1">
					<strong>
						<label for="lblDescription" >Description</label>
					</strong>
				</td>
				<td colspan="7">
					<xsl:if test="(($viewEditType = 'print') or ($viewEditType = 'part')) and ($nodeName = 'outputseries')">
						<xsl:value-of select="@Description" />
					</xsl:if>
					<xsl:if test="($viewEditType = 'full') and ($nodeName = 'outputseries')">
						<textarea class="Text75H100PCW" id="lblDescription">
							<xsl:attribute name="name"><xsl:value-of select="$searchurl" />;Description;string;255</xsl:attribute>
							<xsl:value-of select="@Description" />
						</textarea>
					</xsl:if>
				</td>
			</tr>
			<xsl:if test="(contains($selectedFileURIPattern, '/temp')) or (($nodeName = 'outputseries') and ($viewEditType = 'print' or $viewEditType = 'full')
					and ($serverActionType != 'linkedviews'))">
				<tr>
					<td scope="row" colspan="8">
						<xsl:value-of select="DisplayDevPacks:WriteViewLinks($searchurl, $contenturipattern, $calcParams, local-name(), @OldId)"/>
					</td>
				</tr>
			</xsl:if>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>

  