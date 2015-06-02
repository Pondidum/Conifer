namespace :ci do

  api_url = "https://ci.appveyor.com/api"
  account = "Pondidum"
  headers = {
    :accept => "application/json",
    :content_type => "application/json"
  }

  desc 'Gets the status of the most recent AppVeyor build'
  task :status do

    project_json = JSON.parse(RestClient.get("#{api_url}/projects/#{account}/#{@project_name}", headers))
    build_json = project_json["build"]
    build_number = build_json["buildNumber"]
    job_status =  build_json["jobs"][0]["status"]

    puts "Build #{build_number}, #{job_status}"

  end

  desc 'Downloads the latest successful artifact from AppVeyor'
  task :get do

    project_json = JSON.parse(RestClient.get("#{api_url}/projects/#{account}/#{@project_name}", headers))
    job_id =  project_json["build"]["jobs"][0]["jobId"]

    artifact_json = JSON.parse(RestClient.get("#{api_url}/buildjobs/#{job_id}/artifacts", headers))
    artifact_name = artifact_json[0]["fileName"]

    FileUtils.mkdir_p package_output unless Dir.exists? package_output

    open(artifact_name, "wb") do |file|
      file << open("#{api_url}/buildjobs/#{job_id}/artifacts/#{artifact_name}").read
    end

  end
end
