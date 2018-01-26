import paramiko
import os
import stat
import configparser

parser = configparser.ConfigParser()
parser.read("config.ini")

config = parser['fgj18']
user = config.get('user')
password = config.get('password')
host = config.get('host')
port = config.getint('port')

transport = paramiko.Transport((host, port))
transport.connect(username=user, password=password)
sftp = paramiko.SFTPClient.from_transport(transport)

remote_path = config.get('remotepath')
local_path = config.get('localpath')

sftp.chdir(remote_path)

for dirname, dirnames, filenames in os.walk(local_path):
    relpath = os.path.relpath(dirname, local_path).replace("\\", "/")
    listdir = sftp.listdir()
    sftp.chdir(relpath)
    for filename in filenames:
        filepath = os.path.join(dirname, filename)
        print("Uploading " + relpath + "/" + filename)
        sftp.put(filepath, filename)
    for dirname in dirnames:
        if dirname not in listdir:
            print("mkdir " + dirname)
            sftp.mkdir(dirname)
            mode = stat.S_IRUSR | stat.S_IWUSR | stat.S_IXUSR | stat.S_IRGRP | stat.S_IXGRP | stat.S_IROTH | stat.S_IXOTH
            sftp.chmod(dirname, mode)
    if relpath != ".":
        sftp.chdir("..")

sftp.close()
transport.close()
print('Upload done.')
