<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2014, December -->
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
		<div id="mainwrapper">
			<table class="data" cellpadding="6" cellspacing="1" border="0">
				<tbody>
					<xsl:apply-templates select="servicebase" />
					<xsl:apply-templates select="outputgroup" />
					<tr id="footer">
						<td scope="row" colspan="10">
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
		<tr>
			<th colspan="10">
				Service: <xsl:value-of select="@Name" />
			</th>
		</tr>
		<xsl:apply-templates select="outputgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="servicebase">
		<tr>
			<th colspan="10">
				Service: <xsl:value-of select="@Name" />
			</th>
		</tr>
		<xsl:apply-templates select="outputgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outputgroup">
		<tr>
			<th scope="col" colspan="10">
				Output Group
			</th>
		</tr>
		<tr>
			<td colspan="10">
				<strong><xsl:value-of select="@Name" /> </strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='sbprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="output">
			<xsl:sort select="@OutputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="output">
		<tr>
			<th scope="col" colspan="10"><strong>Output</strong></th>
		</tr>
		<tr>
			<td scope="row" colspan="10">
				<strong><xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='sbprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="outputseries">
		<xsl:sort select="@OutputDate"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="outputseries">
		<tr>
			<td scope="row" colspan="10">
				<strong>Output Series: <xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='sbprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
    <xsl:if test="(@TSB1ScoreMUnit != '')">
      <tr>
        <th>
          Observations ; Label
        </th>
        <th>
          Plan Period
        </th>
        <th>
          Plan Full
        </th>
        <th>
          Plan Cumul
        </th>
        <th>
          Actual Period
        </th>
        <th>
          Actual Cumul
        </th>
        <th>
          Actual Period Change
        </th>
        <th>
          Actual Cumul Change
        </th>
        <th>
          Plan P Percent ; Plan C Percent
        </th>
        <th>
          Plan Full Percent
        </th>
      </tr>
      <tr>
        <td>
				  <xsl:value-of select="@TSB1ScoreN"/>; <xsl:value-of select="@TSB1ScoreMUnit"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSB1ScoreM"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSBScorePFTotal"/>
			  </td>
        <td>
				  <xsl:value-of select="@TSBScorePCTotal"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSBScoreAPTotal"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSBScoreACTotal"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSBScoreAPChange"/>
			  </td>
        <td>
					  <xsl:value-of select="@TSBScoreACChange"/>
			  </td>
        <td>
          <xsl:value-of select="@TSBScorePPPercent"/>; <xsl:value-of select="@TSBScorePCPercent"/>
			  </td>
        <td>
          <xsl:value-of select="@TSBScorePFPercent"/>
			  </td>
		  </tr>
      <tr>
        <td>
				  Low : <xsl:value-of select="@TSB1ScoreLUnit"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSB1ScoreLAmount"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSBScoreLPFTotal"/>
			  </td>
        <td>
				  <xsl:value-of select="@TSBScoreLPCTotal"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSBScoreLAPTotal"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSBScoreLACTotal"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSBScoreLAPChange"/>
			  </td>
        <td>
					  <xsl:value-of select="@TSBScoreLACChange"/>
			  </td>
        <td>
          <xsl:value-of select="@TSBScoreLPPPercent"/>; <xsl:value-of select="@TSBScoreLPCPercent"/>
			  </td>
        <td>
          <xsl:value-of select="@TSBScoreLPFPercent"/>
			  </td>
		  </tr>
      <tr>
        <td>
				  High : <xsl:value-of select="@TSB1ScoreUUnit"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSB1ScoreUAmount"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TSBScoreUPFTotal"/>
			  </td>
        <td>
				  <xsl:value-of select="@TSBScoreUPCTotal"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSBScoreUAPTotal"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSBScoreUACTotal"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TSBScoreUAPChange"/>
			  </td>
        <td>
					  <xsl:value-of select="@TSBScoreUACChange"/>
			  </td>
        <td>
          <xsl:value-of select="@TSBScoreUPPPercent"/>; <xsl:value-of select="@TSBScoreUPCPercent"/>
			  </td>
        <td>
          <xsl:value-of select="@TSBScoreUPFPercent"/>
			  </td>
		  </tr>
    </xsl:if>
     <tr>
      <th>
        Name (Observations)
      </th>
      <th>
        Plan Period
      </th>
      <th>
        Plan Full
      </th>
      <th>
        Plan Cumul
      </th>
      <th>
        Actual Period
      </th>
      <th>
        Actual Cumul
      </th>
      <th>
        Actual Period Change
      </th>
      <th>
        Actual Cumul Change
      </th>
      <th>
        Plan P Percent ; Plan C Percent
      </th>
      <th>
        Plan Full Percent
      </th>
    </tr>
    <tr>
      <td colspan="10">
        Date : <xsl:value-of select="@Date"/> ; Target : <xsl:value-of select="@TargetType"/>
      </td>
    </tr>
    <xsl:if test="(string-length(@TSB1Name1) > 0)">
      <tr>
        <td>
          <xsl:value-of select="@TSB1Name1"/> (<xsl:value-of select="@TSB1N1"/>)
        </td>
        <td>
          <xsl:value-of select="@TSB1TMAmount1"/>
        </td>
        <td>
          <xsl:value-of select="@TSB1PFTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB1PCTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB1APTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB1ACTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB1APChange"/>
        </td>
        <td>
          <xsl:value-of select="@TSB1ACChange"/>
        </td>
        <td>
          <xsl:value-of select="@TSB1PPPercent"/> ; <xsl:value-of select="@TSB1PCPercent"/>
        </td>
        <td>
          <xsl:value-of select="@TSB1PFPercent"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(string-length(@TSB1Name2) > 0)">
      <tr>
        <td>
          <xsl:value-of select="@TSB1Name2"/> (<xsl:value-of select="@TSB1N2"/>)
        </td>
        <td>
          <xsl:value-of select="@TSB1TMAmount2"/>
        </td>
        <td>
          <xsl:value-of select="@TSB2PFTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB2PCTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB2APTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB2ACTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB2APChange"/>
        </td>
        <td>
          <xsl:value-of select="@TSB2ACChange"/>
        </td>
        <td>
          <xsl:value-of select="@TSB2PPPercent"/> ; <xsl:value-of select="@TSB2PCPercent"/>
        </td>
        <td>
          <xsl:value-of select="@TSB2PFPercent"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(string-length(@TSB1Name3) > 0)">
      <tr>
        <td>
          <xsl:value-of select="@TSB1Name3"/> (<xsl:value-of select="@TSB1N3"/>)
        </td>
        <td>
          <xsl:value-of select="@TSB1TMAmount3"/>
        </td>
        <td>
          <xsl:value-of select="@TSB3PFTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB3PCTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB3APTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB3ACTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB3APChange"/>
        </td>
        <td>
          <xsl:value-of select="@TSB3ACChange"/>
        </td>
        <td>
          <xsl:value-of select="@TSB3PPPercent"/> ; <xsl:value-of select="@TSB3PCPercent"/>
        </td>
        <td>
          <xsl:value-of select="@TSB3PFPercent"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(string-length(@TSB1Name4) > 0)">
      <tr>
        <td>
          <xsl:value-of select="@TSB1Name4"/> (<xsl:value-of select="@TSB1N4"/>)
        </td>
        <td>
          <xsl:value-of select="@TSB1TMAmount4"/>
        </td>
        <td>
          <xsl:value-of select="@TSB4PFTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB4PCTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB4APTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB4ACTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB4APChange"/>
        </td>
        <td>
          <xsl:value-of select="@TSB4ACChange"/>
        </td>
        <td>
          <xsl:value-of select="@TSB4PPPercent"/> ; <xsl:value-of select="@TSB4PCPercent"/>
        </td>
        <td>
          <xsl:value-of select="@TSB4PFPercent"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(string-length(@TSB1Name5) > 0)">
      <tr>
        <td>
          <xsl:value-of select="@TSB1Name5"/> (<xsl:value-of select="@TSB1N5"/>)
        </td>
        <td>
          <xsl:value-of select="@TSB1TMAmount5"/>
        </td>
        <td>
          <xsl:value-of select="@TSB5PFTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB5PCTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB5APTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB5ACTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB5APChange"/>
        </td>
        <td>
          <xsl:value-of select="@TSB5ACChange"/>
        </td>
        <td>
          <xsl:value-of select="@TSB5PPPercent"/> ; <xsl:value-of select="@TSB5PCPercent"/>
        </td>
        <td>
          <xsl:value-of select="@TSB5PFPercent"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(string-length(@TSB1Name6) > 0)">
      <tr>
        <td>
          <xsl:value-of select="@TSB1Name6"/> (<xsl:value-of select="@TSB1N6"/>)
        </td>
        <td>
          <xsl:value-of select="@TSB1TMAmount6"/>
        </td>
        <td>
          <xsl:value-of select="@TSB6PFTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB6PCTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB6APTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB6ACTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB6APChange"/>
        </td>
        <td>
          <xsl:value-of select="@TSB6ACChange"/>
        </td>
        <td>
          <xsl:value-of select="@TSB6PPPercent"/> ; <xsl:value-of select="@TSB6PCPercent"/>
        </td>
        <td>
          <xsl:value-of select="@TSB6PFPercent"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(string-length(@TSB1Name7) > 0)">
      <tr>
        <td>
          <xsl:value-of select="@TSB1Name7"/> (<xsl:value-of select="@TSB1N7"/>)
        </td>
        <td>
          <xsl:value-of select="@TSB1TMAmount7"/>
        </td>
        <td>
          <xsl:value-of select="@TSB7PFTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB7PCTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB7APTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB7ACTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB7APChange"/>
        </td>
        <td>
          <xsl:value-of select="@TSB7ACChange"/>
        </td>
        <td>
          <xsl:value-of select="@TSB7PPPercent"/> ; <xsl:value-of select="@TSB7PCPercent"/>
        </td>
        <td>
          <xsl:value-of select="@TSB7PFPercent"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(string-length(@TSB1Name8) > 0)">
      <tr>
        <td>
          <xsl:value-of select="@TSB1Name8"/> (<xsl:value-of select="@TSB1N8"/>)
        </td>
        <td>
          <xsl:value-of select="@TSB1TMAmount8"/>
        </td>
        <td>
          <xsl:value-of select="@TSB8PFTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB8PCTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB8APTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB8ACTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB8APChange"/>
        </td>
        <td>
          <xsl:value-of select="@TSB8ACChange"/>
        </td>
        <td>
          <xsl:value-of select="@TSB8PPPercent"/> ; <xsl:value-of select="@TSB8PCPercent"/>
        </td>
        <td>
          <xsl:value-of select="@TSB8PFPercent"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(string-length(@TSB1Name9) > 0)">
      <tr>
        <td>
          <xsl:value-of select="@TSB1Name9"/> (<xsl:value-of select="@TSB1N9"/>)
        </td>
        <td>
          <xsl:value-of select="@TSB1TMAmount9"/>
        </td>
        <td>
          <xsl:value-of select="@TSB9PFTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB9PCTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB9APTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB9ACTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB9APChange"/>
        </td>
        <td>
          <xsl:value-of select="@TSB9ACChange"/>
        </td>
        <td>
          <xsl:value-of select="@TSB9PPPercent"/> ; <xsl:value-of select="@TSB9PCPercent"/>
        </td>
        <td>
          <xsl:value-of select="@TSB9PFPercent"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(string-length(@TSB1Name10) > 0)">
      <tr>
        <td>
          <xsl:value-of select="@TSB1Name10"/> (<xsl:value-of select="@TSB1N10"/>)
        </td>
        <td>
          <xsl:value-of select="@TSB1TMAmount10"/>
        </td>
        <td>
          <xsl:value-of select="@TSB10PFTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB10PCTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB10APTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB10ACTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TSB10APChange"/>
        </td>
        <td>
          <xsl:value-of select="@TSB10ACChange"/>
        </td>
        <td>
          <xsl:value-of select="@TSB10PPPercent"/> ; <xsl:value-of select="@TSB10PCPercent"/>
        </td>
        <td>
          <xsl:value-of select="@TSB10PFPercent"/>
        </td>
      </tr>
    </xsl:if>
   </xsl:template>
</xsl:stylesheet>
