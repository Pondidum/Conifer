namespace :docs do

  def get_method(file, method)
    rx = Regexp.new(".*" + method + "\(\).*")

    lines = File.readlines(file)

    start_index = lines.index { |line| line =~ rx }

    ending_token = lines[start_index + 1].gsub("{", "}")
    end_index = lines.drop(start_index).index { |line| line == ending_token }

    lines.slice(start_index - 1, end_index + 2).join()

  end

  task :scan do

    open("test.out.md", "w") do |out|
      File.readlines("test.md").each do |line|

        match = /^    \$include (.*?)@(.*)/.match(line)

        if match then

          file = match[1]
          lang = File.extname(match[1])[1..-1]
          method = match[2]

          to_insert = get_method(file, method)

          out.puts "```" + lang
          out.puts to_insert
          out.puts "```"

        else
          out.puts line
        end

      end
    end
  end

end
