Vagrant.configure('2') do |config|
  config.vm.box = 'bento/ubuntu-18.04'
  config.vm.box_check_update = false
  config.vm.hostname = 'docker-stk'
  config.vm.network :forwarded_port, id: 'ssh', host_ip: '127.0.0.1', host: 2200, guest: 22, auto_correct: false
  config.vm.network :private_network, ip: '192.168.8.100'
  config.vm.provider 'virtualbox' do |vb|
    vb.name = 'vm-docker-stk'
    vb.cpus = 2
    vb.memory = 1024
  end
  config.vm.provision :shell, inline: <<-SHELL.gsub(/^ +/, '')
    echo '************************************************** Configuring system settings'
    sudo timedatectl set-timezone Europe/London
    sudo update-locale LANG=en_US.UTF-8 LC_ALL=en_US.UTF-8
    echo '%sudo ALL=(ALL) NOPASSWD: ALL' | sudo tee -a /etc/sudoers
    echo '****************************** Downloading packages'
    sudo apt-get -q -y update
    sudo DEBIAN_FRONTEND=noninteractive apt-get -q -y -o Dpkg::Options::='--force-confdef' -o Dpkg::Options::='--force-confold' upgrade
    sudo apt-get -q -y dist-upgrade
    sudo apt-get -q -y install sudo linux-kernel-headers linux-headers-generic linux-headers-4.15.0-20-generic apt-transport-https ca-certificates software-properties-common curl git tree whois unzip pigz
    echo '************************************************** Updating Guest Additions'
    sudo wget -q -c http://download.virtualbox.org/virtualbox/LATEST.TXT -O /tmp/vbga-latest-version.txt
    vbgaLatestVersion=$(sudo cat /tmp/vbga-latest-version.txt)
    sudo rm /tmp/vbga-latest-version.txt
    sudo wget -q -c http://download.virtualbox.org/virtualbox/$vbgaLatestVersion/VBoxGuestAdditions_$vbgaLatestVersion.iso -O vbga.iso
    sudo mount -o ro -o loop vbga.iso /mnt
    pushd /lib/modules/4.15.0-12-generic/ && sudo ln -sv /usr/src/linux-headers-4.15.0-20-generic build && sudo ln -sv /usr/src/linux-headers-4.15.0-20-generic source && popd
    sudo /mnt/VBoxLinuxAdditions.run && rm /lib/modules/4.15.0-12-generic/build && rm /lib/modules/4.15.0-12-generic/source
    sudo umount /mnt
    sudo rm vbga.iso
    sudo systemctl enable vboxadd && sudo systemctl start vboxadd
    echo '************************************************** Installing Docker'
    echo '********** Adding Docker deb repository'
    curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo apt-key add -
    sudo add-apt-repository 'deb [arch=amd64] https://download.docker.com/linux/ubuntu xenial stable'
    echo '********** Installing Docker Engine'
    sudo apt-get -q -y update
    sudo apt-get -q -y install docker-ce='5:18.09.6~3-0~ubuntu-xenial'
    sudo systemctl enable docker && sudo systemctl start docker
    sudo usermod -aG docker vagrant
    echo '********** Enabling socket and HTTP Docker APIs'
    echo 'Usage:'
    echo "echo -e 'GET /images/json HTTP/1.0\\\\r\\\\n' | netcat -U /var/run/docker.sock"
    echo 'curl localhost:4243/images/json'
    sudo mkdir -p /etc/systemd/system/docker.service.d
    sudo echo '[Service]' | sudo tee /etc/systemd/system/docker.service.d/hosts.conf
    sudo echo 'ExecStart=' | sudo tee -a /etc/systemd/system/docker.service.d/hosts.conf
    sudo echo 'ExecStart=/usr/bin/dockerd -H unix:///var/run/docker.sock -H tcp://0.0.0.0:4243' | sudo tee -a /etc/systemd/system/docker.service.d/hosts.conf
    sudo systemctl daemon-reload
    sudo systemctl restart docker
    echo '********** Installing Docker Compose'
    sudo wget -q https://github.com/docker/compose/releases/download/1.24.0/docker-compose-`uname -s`-`uname -m` -O /usr/local/bin/docker-compose
    sudo chmod +x /usr/local/bin/docker-compose
  SHELL
end