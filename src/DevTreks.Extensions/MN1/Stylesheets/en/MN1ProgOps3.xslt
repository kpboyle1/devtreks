<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2014, June -->
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
		<div id="mainwrapper">
			<table class="data" cellpadding="6" cellspacing="1" border="0">
				<tbody>
					<xsl:apply-templates select="servicebase" />
					<xsl:apply-templates select="operationgroup" />
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
		<xsl:apply-templates select="operationgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="operationgroup">
		<tr>
			<th scope="col" colspan="10">
				Operation Group
			</th>
		</tr>
		<tr>
			<td colspan="10">
				<strong><xsl:value-of select="@Name" /> </strong>
			</td>
		</tr>
    <tr>
      <th>
        Name
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
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mnprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="operation">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="operation">
    <tr>
			<th scope="col" colspan="10"><strong>Operation</strong></th>
		</tr>
		<tr>
			<td scope="row" colspan="10">
				<strong><xsl:value-of select="@Name" />(Amount:&#xA0;<xsl:value-of select="@Amount" />;&#xA0;Date:&#xA0;<xsl:value-of select="@Date" />&#xA0;Label:&#xA0;<xsl:value-of select="@Num" /></strong>
			</td>
		</tr>
    <tr>
      <th>
        Name
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
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mnprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:variable name="count" select="count(operationinput)"/>
    <xsl:if test="($count > 0)">
      <xsl:apply-templates select="operationinput">
			  <xsl:sort select="@InputDate"/>
		  </xsl:apply-templates>
    </xsl:if>
	</xsl:template>
	<xsl:template match="operationinput">
		<tr>
			<td scope="row" colspan="10">
				<strong>Input: <xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='mnprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
    <tr>
      <td colspan="10">
        Date : <xsl:value-of select="@Date"/> ; Observations: <xsl:value-of select="@Observations"/>; Target : <xsl:value-of select="@TargetType"/>
      </td>
    </tr>
    <tr>
      <td>
        <xsl:value-of select="@TMN1Name"/>
      </td>
      <td>
        <xsl:value-of select="@TMN1Q"/>
      </td>
      <td>
        <xsl:value-of select="@TMN1PFTotal"/>
      </td>
      <td>
        <xsl:value-of select="@TMN1PCTotal"/>
      </td>
      <td>
        <xsl:value-of select="@TMN1APTotal"/>
      </td>
      <td>
        <xsl:value-of select="@TMN1ACTotal"/>
      </td>
      <td>
        <xsl:value-of select="@TMN1APChange"/>
      </td>
      <td>
        <xsl:value-of select="@TMN1ACChange"/>
      </td>
      <td>
        <xsl:value-of select="@TMN1PPPercent"/> ; <xsl:value-of select="@TMN1PCPercent"/>
      </td>
      <td>
        <xsl:value-of select="@TMN1PFPercent"/>
      </td>
    </tr>
    <tr>
      <td>
        <xsl:value-of select="@TMN2Name"/>
      </td>
      <td>
        <xsl:value-of select="@TMN2Q"/>
      </td>
      <td>
        <xsl:value-of select="@TMN2PFTotal"/>
      </td>
      <td>
        <xsl:value-of select="@TMN2PCTotal"/>
      </td>
      <td>
        <xsl:value-of select="@TMN2APTotal"/>
      </td>
      <td>
        <xsl:value-of select="@TMN2ACTotal"/>
      </td>
      <td>
        <xsl:value-of select="@TMN2APChange"/>
      </td>
      <td>
        <xsl:value-of select="@TMN2ACChange"/>
      </td>
      <td>
        <xsl:value-of select="@TMN2PPPercent"/> ; <xsl:value-of select="@TMN2PCPercent"/>
      </td>
      <td>
        <xsl:value-of select="@TMN2PFPercent"/>
      </td>
    </tr>
    <xsl:if test="(string-length(@TMN3Name) > 0)">
      <tr>
        <td>
          <xsl:value-of select="@TMN3Name"/>
        </td>
        <td>
          <xsl:value-of select="@TMN3Q"/>
        </td>
        <td>
          <xsl:value-of select="@TMN3PFTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TMN3PCTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TMN3APTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TMN3ACTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TMN3APChange"/>
        </td>
        <td>
          <xsl:value-of select="@TMN3ACChange"/>
        </td>
        <td>
          <xsl:value-of select="@TMN3PPPercent"/> ; <xsl:value-of select="@TMN3PCPercent"/>
        </td>
        <td>
          <xsl:value-of select="@TMN3PFPercent"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(string-length(@TMN4Name) > 0)">
      <tr>
        <td>
          <xsl:value-of select="@TMN4Name"/>
        </td>
        <td>
          <xsl:value-of select="@TMN4Q"/>
        </td>
        <td>
          <xsl:value-of select="@TMN4PFTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TMN4PCTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TMN4APTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TMN4ACTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TMN4APChange"/>
        </td>
        <td>
          <xsl:value-of select="@TMN4ACChange"/>
        </td>
        <td>
          <xsl:value-of select="@TMN4PPPercent"/> ; <xsl:value-of select="@TMN4PCPercent"/>
        </td>
        <td>
          <xsl:value-of select="@TMN4PFPercent"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(string-length(@TMN5Name) > 0)">
      <tr>
        <td>
          <xsl:value-of select="@TMN5Name"/>
        </td>
        <td>
          <xsl:value-of select="@TMN5Q"/>
        </td>
        <td>
          <xsl:value-of select="@TMN5PFTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TMN5PCTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TMN5APTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TMN5ACTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TMN5APChange"/>
        </td>
        <td>
          <xsl:value-of select="@TMN5ACChange"/>
        </td>
        <td>
          <xsl:value-of select="@TMN5PPPercent"/> ; <xsl:value-of select="@TMN5PCPercent"/>
        </td>
        <td>
          <xsl:value-of select="@TMN5PFPercent"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(string-length(@TMN6Name) > 0)">
      <tr>
        <td>
          <xsl:value-of select="@TMN6Name"/>
        </td>
        <td>
          <xsl:value-of select="@TMN6Q"/>
        </td>
        <td>
          <xsl:value-of select="@TMN6PFTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TMN6PCTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TMN6APTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TMN6ACTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TMN6APChange"/>
        </td>
        <td>
          <xsl:value-of select="@TMN6ACChange"/>
        </td>
        <td>
          <xsl:value-of select="@TMN6PPPercent"/> ; <xsl:value-of select="@TMN6PCPercent"/>
        </td>
        <td>
          <xsl:value-of select="@TMN6PFPercent"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(string-length(@TMN7Name) > 0)">
      <tr>
        <td>
          <xsl:value-of select="@TMN7Name"/>
        </td>
        <td>
          <xsl:value-of select="@TMN7Q"/>
        </td>
        <td>
          <xsl:value-of select="@TMN7PFTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TMN7PCTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TMN7APTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TMN7ACTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TMN7APChange"/>
        </td>
        <td>
          <xsl:value-of select="@TMN7ACChange"/>
        </td>
        <td>
          <xsl:value-of select="@TMN7PPPercent"/> ; <xsl:value-of select="@TMN7PCPercent"/>
        </td>
        <td>
          <xsl:value-of select="@TMN7PFPercent"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(string-length(@TMN8Name) > 0)">
      <tr>
        <td>
          <xsl:value-of select="@TMN8Name"/>
        </td>
        <td>
          <xsl:value-of select="@TMN8Q"/>
        </td>
        <td>
          <xsl:value-of select="@TMN8PFTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TMN8PCTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TMN8APTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TMN8ACTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TMN8APChange"/>
        </td>
        <td>
          <xsl:value-of select="@TMN8ACChange"/>
        </td>
        <td>
          <xsl:value-of select="@TMN8PPPercent"/> ; <xsl:value-of select="@TMN8PCPercent"/>
        </td>
        <td>
          <xsl:value-of select="@TMN8PFPercent"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(string-length(@TMN9Name) > 0)">
      <tr>
        <td>
          <xsl:value-of select="@TMN9Name"/>
        </td>
        <td>
          <xsl:value-of select="@TMN9Q"/>
        </td>
        <td>
          <xsl:value-of select="@TMN9PFTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TMN9PCTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TMN9APTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TMN9ACTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TMN9APChange"/>
        </td>
        <td>
          <xsl:value-of select="@TMN9ACChange"/>
        </td>
        <td>
          <xsl:value-of select="@TMN9PPPercent"/> ; <xsl:value-of select="@TMN9PCPercent"/>
        </td>
        <td>
          <xsl:value-of select="@TMN9PFPercent"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="(string-length(@TMN10Name) > 0)">
      <tr>
        <td>
          <xsl:value-of select="@TMN10Name"/>
        </td>
        <td>
          <xsl:value-of select="@TMN10Q"/>
        </td>
        <td>
          <xsl:value-of select="@TMN10PFTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TMN10PCTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TMN10APTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TMN10ACTotal"/>
        </td>
        <td>
          <xsl:value-of select="@TMN10APChange"/>
        </td>
        <td>
          <xsl:value-of select="@TMN10ACChange"/>
        </td>
        <td>
          <xsl:value-of select="@TMN10PPPercent"/> ; <xsl:value-of select="@TMN10PCPercent"/>
        </td>
        <td>
          <xsl:value-of select="@TMN10PFPercent"/>
        </td>
      </tr>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>
