require 'bundler/setup'
require 'albacore'
require 'rest-client'
require 'json'
require 'open-uri'

ci_build = ENV['APPVEYOR_BUILD_VERSION'] ||= "0"

tool_nuget = 'tools/nuget/nuget.exe'
tool_xunit = 'tools/xunit/xunit.console.clr4.exe'

project_name = 'Conifer'
project_version = "1.0.#{ci_build}"

project_output = 'build/bin'
package_output = 'build/deploy'

build_mode = ENV['mode'] ||= "Debug"

desc 'Restore nuget packages for all projects'
nugets_restore :restore do |n|
  n.exe = tool_nuget
  n.out = 'packages'
end

desc 'Set the assembly version number'
asmver :version do |v|

  v.file_path = "#{project_name}/Properties/AssemblyVersion.cs"
  v.attributes assembly_version: project_version,
               assembly_file_version: project_version
end

desc 'Compile all projects'
build :compile do |msb|
  msb.target = [ :clean, :rebuild ]
  msb.prop 'configuration', build_mode
  msb.sln = "#{project_name}.sln"
end

desc 'Run all unit test assemblies'
test_runner :test do |xunit|
  xunit.exe = tool_xunit
  xunit.files = FileList['**/bin/*/*.tests.dll']
  xunit.add_parameter '/silent'
end

desc 'Build all nuget packages'
nugets_pack :pack do |n|

  FileUtils.mkdir_p package_output unless Dir.exists? package_output

  n.configuration = build_mode
  n.exe = tool_nuget
  n.out = package_output

  n.files = FileList["#{project_name}/*.csproj"]

  n.with_metadata do |m|
    m.description = 'Strong Typed, convention based router for Webapi/Mvc'
    m.authors = 'Andy Dote'
    m.project_url = 'https://github.com/pondidum/#{project_name}'
    m.license_url = 'https://github.com/Pondidum/#{project_name}/blob/master/LICENSE.txt'
    m.version = project_version
    m.tags = 'rest router webapi mvc routing convention'
  end

end

namespace :ci do

  api_url = "https://ci.appveyor.com/api"
  account = "Pondidum"
  headers = {
    :accept => "application/json",
    :content_type => "application/json"
  }

  desc 'Gets the status of the most recent AppVeyor build'
  task :status do

    project_json = JSON.parse(RestClient.get("#{api_url}/projects/#{account}/#{project_name}", headers))
    build_json = project_json["build"]
    build_number = build_json["buildNumber"]
    job_status =  build_json["jobs"][0]["status"]

    puts "Build #{build_number}, #{job_status}"

  end

  desc 'Downloads the latest successful artifact from AppVeyor'
  task :get do

    project_json = JSON.parse(RestClient.get("#{api_url}/projects/#{account}/#{project_name}", headers))
    job_id =  project_json["build"]["jobs"][0]["jobId"]

    artifact_json = JSON.parse(RestClient.get("#{api_url}/buildjobs/#{job_id}/artifacts", headers))
    artifact_name = artifact_json[0]["fileName"]

    FileUtils.mkdir_p package_output unless Dir.exists? package_output

    open(artifact_name, "wb") do |file|
      file << open("#{api_url}/buildjobs/#{job_id}/artifacts/#{artifact_name}").read
    end

  end
end

task :default => [ :restore, :version, :compile, :test ]
