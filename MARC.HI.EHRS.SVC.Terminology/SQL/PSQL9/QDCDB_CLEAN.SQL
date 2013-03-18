/* 
 * POSTGRESHR - MARC-HI SHARED SCHEMA - QUICK AND DIRTY CODE RESOLVER
 * VERSION: 3.0
 * AUTHOR: JUSTIN FYFE
 * DATE: APRIL 22, 2011
 * FILES:
 *	QDCODEDB.SQL		- SQL CODE TO CREATE TABLES, AND INDICIES FOR SIMEPL CODE DATABASE
 * LICENSE:
 * 	Licensed under the Apache License, Version 2.0 (the "License");
 * 	you may not use this file except in compliance with the License.
 * 	You may obtain a copy of the License at
 *
 *     		http://www.apache.org/licenses/LICENSE-2.0
 *
 * 	Unless required by applicable law or agreed to in writing, software
 * 	distributed under the License is distributed on an "AS IS" BASIS,
 * 	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * 	See the License for the specific language governing permissions and
 * 	limitations under the License.
 *
 */

 DROP SEQUENCE QDCDB_CNCPT_SEQ CASCADE;
 DROP TABLE QDCDB_CNCPT_CDTBL CASCADE;
 DROP TABLE QDCDB_CNCPT_MAP_TBL CASCADE;
