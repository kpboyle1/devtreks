﻿<?xml version="1.0" encoding="UTF-8" ?>
<!-- Author: www.devtreks.org, 2013, October -->
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
		<div id="mainwrapper">
			<table class="data" cellpadding="6" cellspacing="1" border="0">
				<tbody>
					<xsl:apply-templates select="servicebase" />
					<xsl:apply-templates select="budgetgroup" />
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
		<xsl:apply-templates select="budgetgroup">
			<xsl:sort select="@Name"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetgroup">
    <tr>
			<th scope="col" colspan="10">
				Budget Group : <xsl:value-of select="@Name" /> 
			</th>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcaprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budget">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budget">
		<tr>
			<th scope="col" colspan="10">
				Budget : <xsl:value-of select="@Name" /> 
			</th>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcaprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:apply-templates select="budgettimeperiod">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgettimeperiod">
		<tr>
			<th scope="col" colspan="10">
				Time Period : <xsl:value-of select="@Name" />&#xA0;Label:&#xA0;<xsl:value-of select="@Num" />
			</th>
		</tr>
    <xsl:apply-templates select="root/linkedview[@AnalyzerType='lcaprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <xsl:variable name="outcount" select="count(budgetoutcomes/budgetoutcome)"/>
    <xsl:if test="($outcount > 0)"> 
		  <tr>
			  <th scope="col" colspan="10">Outcomes</th>
		  </tr>
		  <xsl:apply-templates select="budgetoutcomes" />
    </xsl:if>
    <xsl:variable name="opcount" select="count(budgetoperations/budgetoperation)"/>
    <xsl:if test="($opcount > 0)"> 
		  <tr>
			  <th scope="col" colspan="10">Operations</th>
		  </tr>
		  <xsl:apply-templates select="budgetoperations" />
    </xsl:if>
	</xsl:template>
	<xsl:template match="budgetoutcomes">
		<xsl:apply-templates select="budgetoutcome">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoutcome">
		<tr>
			<th scope="col" colspan="10"><strong>Outcome</strong></th>
		</tr>
		<tr>
			<td scope="row" colspan="10">
				<strong><xsl:value-of select="@Name" /></strong>&#xA0;Label:&#xA0;<xsl:value-of select="@Num" />
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcaprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <tr>
      <th>
        Totals
			</th>
      <th>
        ---
			</th>
			<th>
        ---
			</th>
      <th>
				Total Benefit
			</th>
			<th>
				LCB Benefit
			</th>
			<th>
        Unit Benefit
			</th>
      <th>
				EAA Benefit
			</th>
			<th>
			</th>
      <th>
			</th>
			<th>
			</th>
		</tr>
    <xsl:apply-templates select="budgetoutput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoutput">
    <tr>
			<td scope="row" colspan="10">
				<strong>Output : <xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcaprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoperations">
		<xsl:apply-templates select="budgetoperation">
			<xsl:sort select="@Date"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetoperation">
		<tr>
			<th scope="col" colspan="10"><strong>Operation</strong></th>
		</tr>
		<tr>
			<td scope="row" colspan="10">
				<strong><xsl:value-of select="@Name" /></strong>&#xA0;Label:&#xA0;<xsl:value-of select="@Num" />
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcaprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
    <tr>
      <th>
        Totals
			</th>
      <th>
				OC Cost
			</th>
      <th>
				AOH Cost
			</th>
			<th>
				CAP Cost
			</th>
			<th>
				LCC Cost
			</th>
			<th>
        Unit Cost
			</th>
      <th>
				EAA Cost
			</th>
			<th>
			</th>
      <th>
			</th>
			<th>
			</th>
		</tr>
    <xsl:apply-templates select="budgetinput">
    </xsl:apply-templates>
	</xsl:template>
	<xsl:template match="budgetinput">
		<tr>
			<td scope="row" colspan="10">
				<strong>Input : <xsl:value-of select="@Name" /></strong>
			</td>
		</tr>
		<xsl:apply-templates select="root/linkedview[@AnalyzerType='lcaprogress1']">
			<xsl:with-param name="localName"><xsl:value-of select="local-name()" /></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="root/linkedview">
		<xsl:param name="localName" />
    <xsl:if test="($localName != 'budgetinput' and $localName != 'budgetoutput')">
      <xsl:if test="($localName != 'budgetoperation')">
        <tr>
          <th>
            C or B Type
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
            Date : <xsl:value-of select="@Date"/> ; Observations : <xsl:value-of select="@Observations"/>; Target : <xsl:value-of select="@TargetType"/>
          </td>
        </tr>
        <tr>
          <td>
            Ben
          </td>
          <td>
            <xsl:value-of select="@TRBenefit"/>
          </td>
          <td>
            <xsl:value-of select="@TRPFTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TRPCTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TRAPTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TRACTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TRAPChange"/>
          </td>
          <td>
            <xsl:value-of select="@TRACChange"/>
          </td>
          <td>
            <xsl:value-of select="@TRPPPercent"/> ; <xsl:value-of select="@TRPCPercent"/>
          </td>
          <td>
            <xsl:value-of select="@TRPFPercent"/>
          </td>
        </tr>
        <tr>
          <td>
            LCB
          </td>
          <td>
            <xsl:value-of select="@TLCBBenefit"/>
          </td>
          <td>
            <xsl:value-of select="@TLCBPFTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TLCBPCTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TLCBAPTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TLCBACTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TLCBAPChange"/>
          </td>
          <td>
            <xsl:value-of select="@TLCBACChange"/>
          </td>
          <td>
            <xsl:value-of select="@TLCBPPPercent"/> ; <xsl:value-of select="@TLCBPCPercent"/>
          </td>
          <td>
            <xsl:value-of select="@TLCBPFPercent"/>
          </td>
        </tr>
        <tr>
          <td>
            REAA
          </td>
          <td>
            <xsl:value-of select="@TREAABenefit"/>
          </td>
          <td>
            <xsl:value-of select="@TREAAPFTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TREAAPCTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TREAAAPTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TREAAACTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TREAAAPChange"/>
          </td>
          <td>
            <xsl:value-of select="@TREAAACChange"/>
          </td>
          <td>
            <xsl:value-of select="@TREAAPPPercent"/> ; <xsl:value-of select="@TREAAPCPercent"/>
          </td>
          <td>
            <xsl:value-of select="@TREAAPFPercent"/>
          </td>
        </tr>
        <tr>
          <td>
            Unit
          </td>
          <td>
            <xsl:value-of select="@TRUnitBenefit"/>
          </td>
          <td>
            <xsl:value-of select="@TRUnitPFTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TRUnitPCTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TRUnitAPTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TRUnitACTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TRUnitAPChange"/>
          </td>
          <td>
            <xsl:value-of select="@TRUnitACChange"/>
          </td>
          <td>
            <xsl:value-of select="@TRUnitPPPercent"/> ; <xsl:value-of select="@TRUnitPCPercent"/>
          </td>
          <td>
            <xsl:value-of select="@TRUnitPFPercent"/>
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="($localName != 'budgetoutcome')">
        <tr>
          <th>
            C or B Type
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
            Date : <xsl:value-of select="@Date"/> ; Observations: <xsl:value-of select="@Observations"/>; Target : <xsl:value-of select="@TargetType"/>
          </td>
        </tr>
        <tr>
          <td>
            OC
          </td>
          <td>
            <xsl:value-of select="@TOCCost"/>
          </td>
          <td>
            <xsl:value-of select="@TOCPFTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TOCPCTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TOCAPTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TOCACTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TOCAPChange"/>
          </td>
          <td>
            <xsl:value-of select="@TOCACChange"/>
          </td>
          <td>
            <xsl:value-of select="@TOCPPPercent"/> ; <xsl:value-of select="@TOCPCPercent"/>
          </td>
          <td>
            <xsl:value-of select="@TOCPFPercent"/>
          </td>
        </tr>
        <tr>
          <td>
            AOH
          </td>
          <td>
            <xsl:value-of select="@TAOHCost"/>
          </td>
          <td>
            <xsl:value-of select="@TAOHPFTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TAOHPCTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TAOHAPTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TAOHACTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TAOHAPChange"/>
          </td>
          <td>
            <xsl:value-of select="@TAOHACChange"/>
          </td>
          <td>
            <xsl:value-of select="@TAOHPPPercent"/> ; <xsl:value-of select="@TAOHPCPercent"/>
          </td>
          <td>
            <xsl:value-of select="@TAOHPFPercent"/>
          </td>
        </tr>
        <tr>
          <td>
            CAP
          </td>
          <td>
            <xsl:value-of select="@TCAPCost"/>
          </td>
          <td>
            <xsl:value-of select="@TCAPPFTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TCAPPCTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TCAPAPTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TCAPACTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TCAPAPChange"/>
          </td>
          <td>
            <xsl:value-of select="@TCAPACChange"/>
          </td>
          <td>
            <xsl:value-of select="@TCAPPPPercent"/> ; <xsl:value-of select="@TCAPPCPercent"/>
          </td>
          <td>
            <xsl:value-of select="@TCAPPFPercent"/>
          </td>
        </tr>
        <tr>
          <td>
            LCC
          </td>
          <td>
            <xsl:value-of select="@TLCCCost"/>
          </td>
          <td>
            <xsl:value-of select="@TLCCPFTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TLCCPCTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TLCCAPTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TLCCACTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TLCCAPChange"/>
          </td>
          <td>
            <xsl:value-of select="@TLCCACChange"/>
          </td>
          <td>
            <xsl:value-of select="@TLCCPPPercent"/> ; <xsl:value-of select="@TLCCPCPercent"/>
          </td>
          <td>
            <xsl:value-of select="@TLCCPFPercent"/>
          </td>
        </tr>
        <tr>
          <td>
            EAA
          </td>
          <td>
            <xsl:value-of select="@TEAACost"/>
          </td>
          <td>
            <xsl:value-of select="@TEAAPFTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TEAAPCTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TEAAAPTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TEAAACTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TEAAAPChange"/>
          </td>
          <td>
            <xsl:value-of select="@TEAAACChange"/>
          </td>
          <td>
            <xsl:value-of select="@TEAAPPPercent"/> ; <xsl:value-of select="@TEAAPCPercent"/>
          </td>
          <td>
            <xsl:value-of select="@TEAAPFPercent"/>
          </td>
        </tr>
        <tr>
          <td>
            Unit
          </td>
          <td>
            <xsl:value-of select="@TUnitCost"/>
          </td>
          <td>
            <xsl:value-of select="@TUnitPFTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TUnitPCTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TUnitAPTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TUnitACTotal"/>
          </td>
          <td>
            <xsl:value-of select="@TUnitAPChange"/>
          </td>
          <td>
            <xsl:value-of select="@TUnitACChange"/>
          </td>
          <td>
            <xsl:value-of select="@TUnitPPPercent"/> ; <xsl:value-of select="@TUnitPCPercent"/>
          </td>
          <td>
            <xsl:value-of select="@TUnitPFPercent"/>
          </td>
        </tr>
      </xsl:if>
    </xsl:if>
		<xsl:if test="($localName = 'budgetinput')">
      <tr>
        <td>
          Totals
			  </td>
			  <td>
				  <xsl:value-of select="@TOCCost"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TAOHCost"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TCAPCost"/>
			  </td>
			  <td>
					  <xsl:value-of select="@TLCCCost"/>
			  </td>
        <td>
					  <xsl:value-of select="@TUnitCost"/>
			  </td>
        <td>
					  <xsl:value-of select="@TEAACost"/>
			  </td>
        <td colspan="3">
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <strong>Description : </strong><xsl:value-of select="@CalculatorDescription" />
			  </td>
      </tr>
		</xsl:if>
    <xsl:if test="($localName = 'budgetoutput')">
      <tr>
        <td>
			  </td>
        <td>
			  </td>
        <td>
			  </td>
			  <td>
          <xsl:value-of select="@TRBenefit"/>
			  </td>
			  <td>
				  <xsl:value-of select="@TLCBBenefit"/>
			  </td>
			  <td>
          <xsl:value-of select="@TRUnitBenefit"/>
			  </td>
        <td>
          <xsl:value-of select="@TREAABenefit"/>
			  </td>
			  <td colspan="3">
			  </td>
		  </tr>
      <tr>
        <td colspan="10">
          <strong>Description : </strong><xsl:value-of select="@CalculatorDescription" />
			  </td>
      </tr>
    </xsl:if>
    <tr>
      <td>
				<strong>
					SubBOrC Name
				</strong>
			</td>
      <td>
				<strong>
					SubBOrC Amount
				</strong>
			</td>
      <td>
				<strong>
					SubBOrC Unit
				</strong>
			</td>
      <td>
				<strong>
					SubBOrC Price
				</strong>
			</td>
			<td>
				<strong>
					SubBOrC Total
				</strong>
			</td>
      <td>
				<strong>
					SubBOrC Unit Total
				</strong>
			</td>
      <td>
        <strong>
					SubBOrC Label
				</strong>
      </td>
			<td colspan="3">
			</td>
		</tr>
    <tr>
      <td colspan="10"><strong>Benefits</strong></td>
    </tr>
     <xsl:if test="(@TSubP2Name1 != '' and @TSubP2Name1 != 'none')">
		    <tr>
          <td>
					    <xsl:value-of select="@TSubP2Name1"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Amount1"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP2Unit1"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Price1"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Total1"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2TotalPerUnit1"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP2Label1"/>
			    </td>
			    <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP2Description1"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Name2 != '' and @TSubP2Name2 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP2Name2"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Amount2"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP2Unit2"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Price2"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Total2"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2TotalPerUnit2"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP2Label2"/>
			    </td>
			    <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP2Description2"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Name3 != '' and @TSubP2Name3 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP2Name3"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Amount3"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP2Unit3"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Price3"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Total3"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2TotalPerUnit3"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP2Label3"/>
			    </td>
			    <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP2Description3"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Name4 != '' and @TSubP2Name4 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP2Name4"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Amount4"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP2Unit4"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Price4"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Total4"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2TotalPerUnit4"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP2Label4"/>
			    </td>
			    <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP2Description4"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Name5 != '' and @TSubP2Name5 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP2Name5"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Amount5"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP2Unit5"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Price5"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Total5"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2TotalPerUnit5"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP2Label5"/>
			    </td>
			    <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP2Description5"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Name6 != '' and @TSubP2Name6 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP2Name6"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Amount6"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP2Unit6"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Price6"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Total6"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2TotalPerUnit6"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP2Label6"/>
			    </td>
			    <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP2Description6"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Name7 != '' and @TSubP2Name7 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP2Name7"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Amount7"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP2Unit7"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Price7"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Total7"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2TotalPerUnit7"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP2Label7"/>
			    </td>
			    <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP2Description7"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Name8 != '' and @TSubP2Name8 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP2Name8"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Amount8"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP2Unit8"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Price8"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Total8"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2TotalPerUnit8"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP2Label8"/>
			    </td>
			    <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP2Description8"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Name9 != '' and @TSubP2Name9 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP2Name9"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Amount9"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP2Unit9"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Price9"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Total9"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2TotalPerUnit9"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP2Label9"/>
			    </td>
			    <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP2Description9"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP2Name10 != '' and @TSubP2Name10 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP2Name10"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Amount10"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP2Unit10"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Price10"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2Total10"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP2TotalPerUnit10"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP2Label10"/>
			    </td>
			    <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP2Description2"/>
			    </td>
		    </tr>
      </xsl:if>
      <tr>
        <td colspan="10"><strong>Costs</strong></td>
      </tr>
      <xsl:if test="(@TSubP1Name1 != '' and @TSubP1Name1 != 'none')">
		    <tr>
          <td>
					    <xsl:value-of select="@TSubP1Name1"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Amount1"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP1Unit1"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Price1"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Total1"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1TotalPerUnit1"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP1Label1"/>
			    </td>
		      <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP1Description1"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Name2 != '' and @TSubP1Name2 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP1Name2"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Amount2"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP1Unit2"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Price2"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Total2"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1TotalPerUnit2"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP1Label2"/>
			    </td>
			      <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP1Description2"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Name3 != '' and @TSubP1Name3 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP1Name3"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Amount3"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP1Unit3"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Price3"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Total3"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1TotalPerUnit3"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP1Label3"/>
			    </td>
			      <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP1Description3"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Name4 != '' and @TSubP1Name4 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP1Name4"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Amount4"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP1Unit4"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Price4"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Total4"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1TotalPerUnit4"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP1Label4"/>
			    </td>
			      <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP1Description4"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Name5 != '' and @TSubP1Name5 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP1Name5"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Amount5"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP1Unit5"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Price5"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Total5"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1TotalPerUnit5"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP1Label5"/>
			    </td>
			      <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP1Description5"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Name6 != '' and @TSubP1Name6 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP1Name6"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Amount6"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP1Unit6"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Price6"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Total6"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1TotalPerUnit6"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP1Label6"/>
			    </td>
			      <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP1Description6"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Name7 != '' and @TSubP1Name7 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP1Name7"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Amount7"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP1Unit7"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Price7"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Total7"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1TotalPerUnit7"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP1Label7"/>
			    </td>
			      <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP1Description7"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Name8 != '' and @TSubP1Name8 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP1Name8"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Amount8"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP1Unit8"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Price8"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Total8"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1TotalPerUnit8"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP1Label8"/>
			    </td>
			      <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP1Description8"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Name9 != '' and @TSubP1Name9 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP1Name9"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Amount9"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP1Unit9"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Price9"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Total9"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1TotalPerUnit9"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP1Label9"/>
			    </td>
			      <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP1Description9"/>
			    </td>
		    </tr>
      </xsl:if>
      <xsl:if test="(@TSubP1Name10 != '' and @TSubP1Name10 != 'none')">
        <tr>
          <td>
					    <xsl:value-of select="@TSubP1Name10"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Amount10"/>
			    </td>
          <td>
					    <xsl:value-of select="@TSubP1Unit10"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Price10"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1Total10"/>
			    </td>
			    <td>
					    <xsl:value-of select="@TSubP1TotalPerUnit10"/>
			    </td>
			    <td>
              <xsl:value-of select="@TSubP1Label10"/>
			    </td>
			      <td colspan="3">
			    </td>
		    </tr>
        <tr>
          <td colspan="10">
            <xsl:value-of select="@TSubP1Description10"/>
			    </td>
		    </tr>
      </xsl:if>
	</xsl:template>
</xsl:stylesheet>
